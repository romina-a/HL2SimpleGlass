using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class SliderInputHandler : MonoBehaviour
{
    [SerializeField]
    Wobble myWobble;
    [SerializeField]
    float angularCoefMin, angularCoefMax, linearCoefMin, linearCoefMax, maxWobbleMin, maxWobbleMax, recoveryMin, recoveryMax, wobbleFrequencyMin, wobbleFrequencyMax;
    [SerializeField]
    TextMeshPro angularTxt, linearTxt, maxWTxt, recoveryTxt, freqTxt;
    public void sliderSetAngularCoef(SliderEventData eventData)
    {
        float val = (eventData.NewValue * (angularCoefMax - angularCoefMin) + angularCoefMin);
        myWobble.setAngularCoef(val);
        angularTxt.text = "AngularCoef: " + val.ToString();
    }
    public void sliderSetLinearCoef(SliderEventData eventData)
    {
        float val = (eventData.NewValue * (linearCoefMax - linearCoefMin) + linearCoefMin);
        myWobble.setLinearCoef(val);
        linearTxt.text = "LinearCoef: " + val.ToString();
    }
    public void sliderSetMaxWobble(SliderEventData eventData)
    {
        float val = (eventData.NewValue * (maxWobbleMax - maxWobbleMin) + maxWobbleMin);
        myWobble.setMaxWobble(val);
        maxWTxt.text = "MaxWobble: " + val.ToString();
    }
    public void sliderSetRecovery(SliderEventData eventData)
    {
        float val = (eventData.NewValue * (recoveryMax - recoveryMin) + recoveryMin);
        myWobble.setRecovery(val);
        recoveryTxt.text = "recovery: " + val.ToString();
    }
    public void sliderSetWobbleFrequency(SliderEventData eventData)
    {
        float val = (eventData.NewValue * (wobbleFrequencyMax - wobbleFrequencyMin) + wobbleFrequencyMin);
        myWobble.setWobbleFrequency(val);
        freqTxt.text = "Frequency: " + val.ToString();
    }
}
