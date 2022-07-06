using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        int i = 100;

        //Debug.Log("i = " + i);
        Debug.Log($"i = {i}");  //こっち使えよ($を使った文字列補間)
        //Debug.Log(string.Format("i = {0}", i));   $の方と同義

        string s = "文字列";
        s = "Stand by Ready";   //上書き可能

        var v = 1234;   //初期値から型推論してくれる

        object obj = null;  //なんでも代入できる型（型安全でない）
        obj = true;
        obj = 1234;
        obj = "Yes";

        //int[] array;    //int型配列の宣言
        //array = new int[3];   //３要素のint型配列を生成
        //array[0] = 10;
        //array[1] = 20;
        //array[2] = 30;

        //Debug.Log($"array[0] = {array[0]}");
        //Debug.Log($"array[1] = {array[1]}");
        //Debug.Log($"array[2] = {array[2]}");

        //for(int ii = 0; ii < array.Length; ii++)
        //{
        //    Debug.Log($"array[{ii}] = {array[ii]}");
        //}

        int[] array = new[] {10,20,30,40,50};    //要素から型推論可能であれば要素の型は不要・[]の数も推論可能

        //for(int ii = 0; ii < array.Length; ii++)    //Lengthプロパティ: 配列の長さを取得
        //{
        //    Debug.Log($"array[{ii}] = {array[ii]}");
        //}

        foreach(var ii in s)
        {
            Debug.Log($"ii = {ii}");
        }
    }
}
