using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceWindow : MonoBehaviour
{
    //Ratio for Scale 1 = 2.35f, look at Root Canvas Scaling
    const float kmRatio = 0.1175f; //using Scale 20
    const float kmToMile = 0.665f;
    const float normalKmPerDay = 39;
    const float slowKmPerDay = 52;
    const float fastKmPerDay = 26;

    const float normalKmPerHour = 4.825f;
    const float fastKmPerHour = 6.4375f;
    const float slowKmPerHour = 3.21875f;

    //Miles
    //24, 32, 16
    //3,4,2

    //km
    //38,6 51,5 25,75
    //4,825 6,4375 3,22

    const int TravelHoursPerDay = 8;
    const int MinutesPerDay = TravelHoursPerDay * MinutesPerHour;
    const int MinutesPerHour = 60;

    [SerializeField] Button closeButton;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI slowTimeText;
    [SerializeField] TextMeshProUGUI normalTimeText;
    [SerializeField] TextMeshProUGUI fastTimeText;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => {
            DistanceManager.InstantReset();
            WindowController.CloseDistanceWindow();
        });
    }

    public void SetData(float distance)
    {
        var kmDistance = Mathf.RoundToInt(distance / kmRatio);
        distanceText.text = kmDistance + "km / " + Mathf.RoundToInt(kmDistance * kmToMile) + " miles";

        var normalTime = CalculateTravelTime(kmDistance, normalKmPerHour * TravelHoursPerDay, normalKmPerHour, normalKmPerHour / 60f);
        normalTimeText.text = "Normal (300ft) " + normalTime.days + "d " + normalTime.hours + "h";

        var slowTime = CalculateTravelTime(kmDistance, slowKmPerHour * TravelHoursPerDay, slowKmPerHour, slowKmPerHour / 60f);
        slowTimeText.text = "Slow (200ft) " + slowTime.days + "d " + slowTime.hours + "h";

        var fastTime = CalculateTravelTime(kmDistance, fastKmPerHour * TravelHoursPerDay, fastKmPerHour, fastKmPerHour / 60f);
        fastTimeText.text = "Fast (400ft) " + fastTime.days + "d " + fastTime.hours + "h";
    }

    (int days, int hours, int minutes) CalculateTravelTime(float distance, float perDay, float perHour, float perMinute)
    {
        int days = Mathf.FloorToInt(distance / perDay);
        var restDistance = distance - days * perDay;
        int hours = Mathf.FloorToInt(restDistance / perHour);
        restDistance = restDistance - hours * perHour;
        return (days, hours, Mathf.FloorToInt(restDistance / perMinute));
    }
}
