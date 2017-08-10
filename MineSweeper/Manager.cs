using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeper
{
    public class Manager
    {
        private readonly Random _random;

        private bool _started;

        public Manager(int length, int height, int bombs)
        {
            _random = new Random();
            MineFields = new List<MineField>();
            GenerateMineField(length, height, bombs);
            _started = true;
        }

        public int Length { get; private set; }
        public int Height { get; private set; }
        public int Bombs { get; private set; }
        public List<MineField> MineFields { get; }

        private void GenerateMineField(int length, int height, int bombs)
        {
            Length = length;
            Height = height;
            Bombs = bombs;

            for (var x = 0; x < length; x++)
                for (var y = 0; y < height; y++)
                    MineFields.Add(new MineField {X = x, Y = y, HasBomb = false});
        }

        public void SetBombZones()
        {
            for (var x = 0; x < Length; x++)
                for (var y = 0; y < Height; y++)
                {
                    var mineField = MineFields.First(f => (f.X == x) && (f.Y == y));

                    if (!mineField.HasBomb)
                        mineField.BombsNearMe = BombCount(x, y);
                }
        }

        public void PickField(int x, int y)
        {
            var mineField = MineFields.First(field => (field.X == x) && (field.Y == y));

            if (!mineField.HasFlag)
                mineField.IsOpenField = true;

            if (_started)
            {
                for (var i = 0; i < Bombs; i++)
                    SetBomb(Length, Height);

                SetBombZones();

                ClearArea(x, y);

                _started = false;
            }

            if (!mineField.HasBomb && (mineField.BombsNearMe == 0))
                ClearArea(x, y);
        }

        private bool CheckOutOfBounds(int x, int y)
        {
            return (x < 0) || (x > Length - 1) || (y > Height - 1) || (y < 0);
        }

        private void ClearArea(int x, int y)
        {
            for (var i = -1; i < 2; i++)
                for (var j = -1; j < 2; j++)
                {
                    if (CheckOutOfBounds(x + i, y + j))
                        continue;

                    var mineField = MineFields.First(f => (f.X == x + i) && (f.Y == y + j));

                    if (mineField.HasBomb || mineField.HasFlag || mineField.IsOpenField)
                        continue;

                    mineField.IsOpenField = true;

                    if (mineField.BombsNearMe <= 1)
                        ClearArea(mineField.X, mineField.Y);
                }
        }

        private int BombCount(int x, int y)
        {
            var bombCounter = 0;
            for (var i = -1; i < 2; i++)
                for (var j = -1; j < 2; j++)
                {
                    if (CheckOutOfBounds(x + i, y + j) || ((i == 0) && (j == 0)))
                        continue;

                    if (MineFields.First(f => (f.X == x + i) && (f.Y == y + j)).HasBomb)
                        bombCounter++;
                }
            return bombCounter;
        }

        public void PlantFlag(int x, int y)
        {
            var mineField = MineFields.First(field => (field.X == x) && (field.Y == y));

            if (!mineField.IsOpenField)
                mineField.HasFlag = !mineField.HasFlag;
        }

        public bool MineExploded()
        {
            return MineFields.Any(x => x.HasExploded());
        }

        public bool Finished()
        {
            return MineFields.Count(f => f.IsOpenField) == MineFields.Count - Bombs;
        }

        private void SetBomb(int legnth, int height)
        {
            while (true)
            {
                var point = new Point(_random.Next(0, legnth - 1), _random.Next(0, height - 1));


                var mineField = MineFields.First(field => (field.X == point.X) && (field.Y == point.Y));

                if (mineField.HasBomb || mineField.IsOpenField)
                    continue;

                MineFields.First(field => (field.X == point.X) && (field.Y == point.Y)).HasBomb = true;

                break;
            }
        }

        public int BombsLeft()
        {
            return Bombs - MineFields.Count(x => x.HasFlag);
        }
    }
}