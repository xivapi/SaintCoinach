﻿using System;
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
                5, Language.ChineseSimplified /*"chs"*/
            }, {
                6, Language.ChineseTraditional /*"cht"*/
            }, {
                7, Language.Korean /*"ko"*/
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
        public int Variant { get; private set; }
        public IEnumerable<Column> Columns { get { return _Columns; } }
        public int ColumnCount {  get { return _Columns.Length; } }
        public IEnumerable<Range> DataFileRanges { get { return _DataFileRanges; } }
        public IEnumerable<Language> AvailableLanguages { get { return _AvailableLanguages; } }
        public int AvailableLanguagesCount { get { return _AvailableLanguages.Count(x => x != Language.None); } }
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
            const int VariantOffset = 0x10;
            const int DataOffset = 0x20;

            var buffer = File.GetData();

            if (buffer.Length < MinimumLength)
                throw new InvalidDataException("EXH file is too short");
            if (OrderedBitConverter.ToUInt32(buffer, 0, false) != Magic)
                throw new InvalidDataException("File not a EX header");


            FixedSizeDataLength = OrderedBitConverter.ToUInt16(buffer, FixedSizeDataLengthOffset, true);
            Variant = OrderedBitConverter.ToUInt16(buffer, VariantOffset, true);
            if (Variant != 1 && Variant != 2)
                throw new NotSupportedException();

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

            var sortedColumns = _Columns.ToList();
            sortedColumns.Sort((a, b) =>
            {
                var aOffset = a.Offset;
                var bOffset = b.Offset;

                var aBits = aOffset * 8;
                var bBits = bOffset * 8;

                var aPosBits = a.Type - 0x19;
                var bPosBits = b.Type - 0x19;

                if (aPosBits > 0) aBits += aPosBits;
                if (bPosBits > 0) bBits += bPosBits;

                return aBits.CompareTo(bBits);
            });

            for (int i = 0; i < sortedColumns.Count; i++)
            {
                var remap = sortedColumns[i].ColumnBasedIndex;
                _Columns[remap].OffsetBasedIndex = i;
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

            // ScreenImage and CutScreenImage reference localized image files,
            // however their available languages are only None.  Perhaps there
            // is a flag to use a global list of available languages in this
            // buffer?

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
