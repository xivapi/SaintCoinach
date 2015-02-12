using System.Collections.Generic;
using System.IO;
using System.Linq;

using File = SaintCoinach.IO.File;

namespace SaintCoinach.Ex {
    public class Header {
        #region Static

        protected static readonly Dictionary<int, Language> LanguageMap = new Dictionary<int, Language> {
            {
                0, Language.None
            }, {
                1, Language.Japanese
            }, {
                2, Language.English
            }, {
                3, Language.German
            }, {
                4, Language.French
            }, {
                5, Language.Unsupported /*"chs"*/
            }, {
                6, Language.Unsupported /*"cht"*/
            }, {
                7, Language.Unsupported /*"ko"*/
            }
        };

        #endregion

        #region Fields

        private Language[] _AvailableLanguages;
        private Column[] _Columns;
        private Range[] _DataFileRanges;

        #endregion

        #region Properties

        public ExCollection Collection { get; private set; }
        public File File { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Column> Columns { get { return _Columns; } }
        public IEnumerable<Range> DataFileRanges { get { return _DataFileRanges; } }
        public IEnumerable<Language> AvailableLanguages { get { return _AvailableLanguages; } }
        public int FixedSizeDataLength { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public Header(ExCollection collection, string name, File file) {
            Collection = collection;
            Name = name;
            File = file;

            Build();
        }

        #endregion

        #endregion

        public Column GetColumn(int index) {
            return _Columns[index];
        }

        #region Factory

        protected virtual Column CreateColumn(int index, byte[] data, int offset) {
            return new Column(this, index, data, offset);
        }

        #endregion

        #region Build

        private void Build() {
            const uint Magic = 0x46485845; // EXHF
            const int MinimumLength = 0x2E;
            const int FixedSizeDataLengthOffset = 0x06;
            const int DataOffset = 0x20;

            var buffer = File.GetData();

            if (buffer.Length < MinimumLength)
                throw new InvalidDataException("EXH file is too short");
            if (OrderedBitConverter.ToUInt32(buffer, 0, false) != Magic)
                throw new InvalidDataException("File not a EX header");

            FixedSizeDataLength = OrderedBitConverter.ToUInt16(buffer, FixedSizeDataLengthOffset, true);

            var currentPosition = DataOffset;
            ReadColumns(buffer, ref currentPosition);
            ReadPartialFiles(buffer, ref currentPosition);
            ReadSuffixes(buffer, ref currentPosition);
        }

        private void ReadColumns(byte[] buffer, ref int position) {
            const int CountOffset = 0x08;
            const int Length = 0x04;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            _Columns = new Column[count];
            for (var i = 0; i < count; ++i) {
                _Columns[i] = CreateColumn(i, buffer, position);
                position += Length;
            }
        }

        private void ReadPartialFiles(byte[] buffer, ref int position) {
            const int CountOffset = 0x0A;
            const int Length = 0x08;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            _DataFileRanges = new Range[count];
            for (var i = 0; i < count; ++i) {
                var min = OrderedBitConverter.ToInt32(buffer, position + 0x00, true);
                var len = OrderedBitConverter.ToInt32(buffer, position + 0x04, true);

                _DataFileRanges[i] = new Range(min, len);

                position += Length;
            }
        }

        private void ReadSuffixes(byte[] buffer, ref int position) {
            const int CountOffset = 0x0C;
            const int Length = 0x02;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            var langs = new List<Language>();
            for (var i = 0; i < count; ++i) {
                langs.Add(LanguageMap[buffer[position]]);
                position += Length;
            }
            _AvailableLanguages = langs.Where(_ => _ != Language.Unsupported).ToArray();
        }

        #endregion
    }
}
