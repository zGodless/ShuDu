using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 数独
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }

        private int[][] questionList = new int[9][];    //题目
        private int[][] answerList = new int[9][];      //答案
        private void Button1_Click(object sender, EventArgs e)
        {
            questionList = new int[9][];
            answerList = new int[9][];
            //填充题目数组
            for (int i = 0; i < 9; i++)
            {
                questionList[i] = new int[9];
                answerList[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    foreach (Control control in panel1.Controls)
                    {
                        if (Convert.ToInt32(control.Tag) == i*9+j+1)
                        {
                            questionList[i][j] = Convert.ToInt32(control.Text == ""? "0": control.Text);
                            answerList[i][j] = Convert.ToInt32(control.Text == "" ? "0" : control.Text);
                        }
                    }
                }
            }
            //计算
            if (calculate())
            {
                //打印结果
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        foreach (Control control in panel2.Controls)
                        {
                            if (Convert.ToInt32(control.Tag) == i * 9 + j + 1 + 81)
                            {
                                control.Text = answerList[i][j].ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        foreach (Control control in panel1.Controls)
                        {
                            if (Convert.ToInt32(control.Tag) == i * 9 + j + 1)
                            {
                                control.Text = answerList[i][j].ToString();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 计算结果
        /// </summary>
        private bool calculate()
        {
            bool isEnd = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (answerList[i][j] == 0)
                    {
                        isEnd = false;
                        //获取可填值数组
                        List<int> posibleList = checkPosible(i, j);
                        if (posibleList.Count > 0)
                        {
                            for (int k = 0; k < posibleList.Count; k++)
                            {
                                answerList[i][j] = posibleList[k];
                                if (calculate())
                                {
                                    return true;
                                }
                                else
                                {
                                    //posibleList[k]解不通，当前位置重置为0，走下一个posibleList[k+1]
                                    answerList[i][j] = 0;
                                    //尝试全部可能解都行不通，返回false表示无可能解
                                    if (k+1 == posibleList.Count)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        //无可能解，回溯
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            if (isEnd)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查当前格可填数值
        /// </summary>
        /// <param name="m">行</param>
        /// <param name="n">列</param>
        /// <returns>返回可填值数组</returns>
        private List<int> checkPosible(int m, int n)
        {
            if (m == 1)
            {

            }
            List<int> posibleList = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                bool flag = true;
                //检查大行、大列是否有重复值
                for (int j = 0; j < 9; j++)
                {
                    if (answerList[m][j] == i || answerList[j][n] == i)
                    {
                        flag = false;
                    }
                }
                //检查小宫是否有重复值
                int min_N = n / 3 * 3;   //宫最小列号
                int min_M = m / 3 * 3;   //宫最小行号
                int max_N = n / 3 * 3 + 2;   //宫最大列号
                int max_M = m / 3 * 3 + 2;   //宫最大行号
                for (int j = min_M; j <= max_M; j++)
                {
                    for (int k = min_N; k <= max_N; k++)
                    {
                        if (answerList[j][k] == i)
                        {
                            flag = false;
                        }
                    }
                }

                if (flag)
                {
                    posibleList.Add(i);
                }
            }

            return posibleList;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel1.Controls)
            {
                control.Text = "";
            }
            foreach (Control control in panel2.Controls)
            {
                control.Text = "";
            }
        }
    }
}
