using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Xiv {
    public class RecipeLevel {
        #region Fields
        private RecipeLevelTable _RecipeLevelTable;
        private int _Difficulty;
        private int _Quality;
        private int _Durability;
        #endregion

        #region Properties
        public RecipeLevelTable RecipeLevelTable { get { return _RecipeLevelTable; } }
        public int Difficulty { get { return _Difficulty; } }
        public int Quality { get { return _Quality; } }
        public int Durability { get { return _Durability; } }
        #endregion

        #region Constructor
        public RecipeLevel(Recipe recipe) {
            _RecipeLevelTable = recipe.RecipeLevelTable;
            _Difficulty = (RecipeLevelTable.Difficulty * recipe.DifficultyFactor) / 100;
            _Quality = (RecipeLevelTable.Quality * recipe.QualityFactor) / 100;
            _Durability = (RecipeLevelTable.Durability * recipe.DurabilityFactor) / 100;
        }
        #endregion
    }
}
