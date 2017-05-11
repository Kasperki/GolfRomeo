using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionUI : MonoBehaviour
{
    public Text JoinInfo;

    public Text PlayerName;
    public Text CarName;
    private int carIndex;
    private bool left, right;

    private ControllerScheme scheme;
    private Player player;

    public bool IsControllerSchemeSame(ControllerScheme scheme)
    {
        if (this.scheme == null)
        {
            return false;
        }

        return this.scheme.Select == scheme.Select;
    }

	public void Join(Player player)
    {
        this.player = player;
        this.scheme = player.ControllerScheme;
        PlayerName.text = player.Name;

        JoinInfo.gameObject.SetActive(false);
        CarName.gameObject.SetActive(true);
        PlayerName.gameObject.SetActive(true);

        UpdateCar();
    }

    public void Leave()
    {
        scheme = null;

        JoinInfo.gameObject.SetActive(true);
        CarName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
    }
	
	void Update ()
    {
        if (scheme != null)
        {
            if (Input.GetAxisRaw(scheme.HorizontalAxis) == 0)
            {
                left = false;
                right = false;
            }

            if (Input.GetAxisRaw(scheme.HorizontalAxis) == -1 && !left)
            {
                carIndex--;

                if (carIndex < 0)
                {
                    carIndex = Enum.GetNames(typeof(CarType)).Length - 1;
                }

                UpdateCar();
                left = true;
            }

            if (Input.GetAxisRaw(scheme.HorizontalAxis) == 1 && !right)
            {
                carIndex++;

                if (carIndex >= Enum.GetNames(typeof(CarType)).Length)
                {
                    carIndex = 0;
                }

                UpdateCar();
                right = true;
            }
        }
    }

    private void UpdateCar()
    {
        player.CarType = (CarType)carIndex;
        CarName.text = Enum.GetName(typeof(CarType), carIndex);
    }
}
