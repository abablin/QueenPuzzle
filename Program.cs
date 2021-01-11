using System;
using System.Collections.Generic;

namespace QueenPuzzleQuestion
{
    class Program
    {
        static void Main(string[] args)
        {
            new QueenPuzzle().Start(8);
        }
    }

    public class QueenPuzzle
    {
        private List<int> lstYIndex=new List<int>();
        private List<string> lstSingleSolution= new List<string>();
        private List<string> lstTotalSolutions = new List<string>();
        
        public void Start(int dimension)
        {           
            // 這兩個迴圈用來跑第一個節點的位置可能性
            // 假如維度是 5, 就會有 5*5 的位置可能性
            for (byte x = 0; x < dimension; x++)
            {
                for (byte y = 0; y < dimension; y++)
                {
                    this.lstYIndex.Add(y);
                    this.lstSingleSolution.Add($"{x}{y}");

                    this.SetNextPosition(x, y, dimension);
                    
                    this.lstSingleSolution.Remove($"{x}{y}");
                    this.lstYIndex.Remove(y);
                }
            }


            // 輸出最後結果
            for (int i = 0; i < this.lstTotalSolutions.Count; i++)
            {
                Console.WriteLine($"Solution {i+1}"); ;
                
                for (byte x = 0; x < dimension; x++)
                {
                    for (byte y = 0; y < dimension; y++)
                    {
                        Console.Write(this.lstTotalSolutions[i].IndexOf($"{x}{y}") > -1 ? "Q" : ".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\r\n");
            }
        }

        /// <summary>
        /// 設定下一個節點的位置
        /// </summary>
        /// <param name="x">當前節點的x軸位置</param>
        /// <param name="y">當前節點的y軸位置</param>
        /// <param name="dimension">維度大小</param>
        private void SetNextPosition(int x, int y,int dimension)
        {
            int nextX,nextY;

            // 如果y軸包含的節點個數=維度值,表示已跑完該維度,
            // 就要判斷該組合的節點有效性, 然後返回執行下一個組合的判斷
            if (this.lstYIndex.Count == dimension)
            {
                DoPositionChecking();
                return;
            }

            nextX = x == (dimension - 1) ? 0 : (x + 1);

            for (nextY = 0; nextY < dimension; nextY++)
            {
                if (this.lstYIndex.Contains(nextY))
                {
                    // 如果下一個節點的y軸, 與當前節點的y軸位於同一條線上, 直接忽略
                    continue;
                }
                else
                {
                    this.lstYIndex.Add(nextY);
                    this.lstSingleSolution.Add($"{nextX}{nextY}");

                    this.SetNextPosition(nextX, nextY, dimension);

                    this.lstYIndex.Remove(nextY);
                    this.lstSingleSolution.Remove($"{nextX}{nextY}");
                }
            }
        }

        /// <summary>
        /// 進行節點有效性檢查, 若通過則加入集合
        /// </summary>
        private void DoPositionChecking()
        {
            // 先判斷是否通過條件判斷
            // 若通過, 把組合中的節點由小到大排序, 最後加到集合(須判斷是否重複)
            if (!ContainSlashPositions())
            {
                this.lstSingleSolution.Sort();

                if (!this.lstTotalSolutions.Contains(string.Join(';', this.lstSingleSolution)))
                {
                    this.lstTotalSolutions.Add(string.Join(';', this.lstSingleSolution));
                }
            }
        }

        /// <summary>
        /// 確認是否有任意兩個節點位於同一條斜線上
        /// </summary>
        /// <returns>true:有 , false:沒有</returns>
        private bool ContainSlashPositions()
        {
            // 利用兩個節點之間的x軸距離,y軸距離 是否相同作為條件
            // 距離相同表示兩個節點位於同一條斜線上
            int currentX, currentY, nextX, nextY;
            bool contain = false;

            for (int i = 0; i < this.lstSingleSolution.Count - 1; i++)
            {
                for (int j = i + 1; j < this.lstSingleSolution.Count; j++)
                {
                    currentX = int.Parse(this.lstSingleSolution[i].Substring(0, 1));
                    currentY = int.Parse(this.lstSingleSolution[i].Substring(1, 1));
                    nextX = int.Parse(this.lstSingleSolution[j].Substring(0, 1));
                    nextY = int.Parse(this.lstSingleSolution[j].Substring(1, 1));

                    if (Math.Abs(currentX - nextX) == Math.Abs(currentY - nextY))
                    {
                        contain = true;
                        i = this.lstSingleSolution.Count;
                        break;
                    }
                }
            }
            return contain;
        }
    } // end of class
}
