using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample2 : MonoBehaviour
{
    //2�����z��

    //�v�f����ׂ邾���Ƃ����_�͒ʏ�̔z��Ɠ���
    //�f�[�^�A�N�Z�X�Ɋe�����̃C���f�b�N�X�𕪗��o����̂�
    //�\�̂悤�ȍ\���̃f�[�^��\�����₷��

    //2�����z��̐錾
    int[,] iAry;

    private void Start()
    {
        //2�����z��̐���
        iAry = new int[5, 3];

        //�e�����̗v�f���擾������@
        //�z��.GetLength(����)

        //�z��S�̗̂v�f�����擾������@
        //�z��.Length

        //�z��̎��������擾����
        //�z��.Rank

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
