using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample2 : MonoBehaviour
{
    //2次元配列

    //要素を並べるだけという点は通常の配列と同じ
    //データアクセスに各次元のインデックスを分離出来るので
    //表のような構造のデータを表現しやすい

    //2次元配列の宣言
    int[,] iAry;

    private void Start()
    {
        //2次元配列の生成
        iAry = new int[5, 3];

        //各次元の要素を取得する方法
        //配列名.GetLength(次元)

        //配列全体の要素数を取得する方法
        //配列名.Length

        //配列の次元数を取得する
        //配列名.Rank

        for(int i = 0; i < iAry.GetLength(0); i++)
        {
            for(int j = 0; j < iAry.GetLength(1); j++)
            {
                iAry[i, j] = i * 10 + j;
                Debug.Log($"{i},{j} = {iAry[i,j]}");
            }
        }
    }
}
