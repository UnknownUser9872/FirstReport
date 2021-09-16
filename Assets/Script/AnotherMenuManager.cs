using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherMenuManager : MonoBehaviourPunCallbacks
{
    public static AnotherMenuManager Instance;//�ٸ� class������ ȣ�Ⱑ��

    [SerializeField] AnotherMenu[] menus;//SerializedField�� ����ϸ� �츮�� publicó�� �� �� ������  public�� �ƴϿ��� �ܺο����� ������.

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string�� �޾Ƽ� �ش��̸� ���� �޴��� ���� ��ũ��Ʈ
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(AnotherMenu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(AnotherMenu menu)
    {
        menu.Close();
    }
}