using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        int i = 100;

        //Debug.Log("i = " + i);
        Debug.Log($"i = {i}");  //�������g����($���g������������)
        //Debug.Log(string.Format("i = {0}", i));   $�̕��Ɠ��`

        string s = "������";
        s = "Stand by Ready";   //�㏑���\

        var v = 1234;   //�����l����^���_���Ă����

        object obj = null;  //�Ȃ�ł�����ł���^�i�^���S�łȂ��j
        obj = true;
        obj = 1234;
        obj = "Yes";

        //int[] array;    //int�^�z��̐錾
        //array = new int[3];   //�R�v�f��int�^�z��𐶐�
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

        int[] array = new[] {10,20,30,40,50};    //�v�f����^���_�\�ł���Ηv�f�̌^�͕s�v�E[]�̐������_�\

        //for(int ii = 0; ii < array.Length; ii++)    //Length�v���p�e�B: �z��̒������擾
        //{
        //    Debug.Log($"array[{ii}] = {array[ii]}");
        //}

        foreach(var ii in s)
        {
            Debug.Log($"ii = {ii}");
        }
    }
}
