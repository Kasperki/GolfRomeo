using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionUI : MonoBehaviour
{
    public Text JoinInfo;

    public Button PreviousCarButton, NextCarButton;
    public Text PlayerName;
    public Text CarName;
    private int carIndex;
    private bool left, right;

    private ControllerScheme scheme;
    private Player player = null;

    public Player Player
    {
        get { return player; }
    }

    public bool IsControllerSchemeSame(ControllerScheme scheme)
    {
        if (this.scheme == null)
        {
            return false;
        }

        return this.scheme.Select == scheme.Select;
    }

    public bool IsSlotAI()
    {
        return player != null && player.PlayerType == PlayerType.AI;
    }

    public bool IsSlotEmpty()
    {
        return player == null;
    }

	public void Join(Player player)
    {
        this.player = player;
        this.scheme = player.ControllerScheme;
        PlayerName.text = player.Name;

        JoinInfo.gameObject.SetActive(false);
        CarName.gameObject.SetActive(true);
        PlayerName.gameObject.SetActive(true);
        PreviousCarButton.gameObject.SetActive(true);
        NextCarButton.gameObject.SetActive(true);

        UpdateCar();
    }

    public void Leave()
    {
        player = null;
        scheme = null;

        JoinInfo.gameObject.SetActive(true);
        CarName.gameObject.SetActive(false);
        PlayerName.gameObject.SetActive(false);
        PreviousCarButton.gameObject.SetActive(false);
        NextCarButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (scheme != null && scheme.HorizontalAxis != new ControllerScheme().Keyboard().HorizontalAxis && GameManager.CheckState(State.Menu))
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

    public void PreviousCar()
    {
        carIndex--;

        if (carIndex < 0)
        {
            carIndex = Enum.GetNames(typeof(CarType)).Length - 1;
        }

        UpdateCar();
    }

    public void NextCar()
    {
        carIndex++;

        if (carIndex >= Enum.GetNames(typeof(CarType)).Length)
        {
            carIndex = 0;
        }

        UpdateCar();
    }

    private void UpdateCar()
    {
        player.CarType = (CarType)carIndex;
        CarName.text = Enum.GetName(typeof(CarType), carIndex);
    }
}
