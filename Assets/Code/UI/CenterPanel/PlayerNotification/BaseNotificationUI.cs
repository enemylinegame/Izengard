﻿using System;
using System.Threading.Tasks;
using Code.UI;
using TMPro;
using UnityEngine;

namespace Code
{
    public sealed class BaseNotificationUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text BasicText;
        [field: SerializeField] public TMP_Text SecondaryText;
        
    }
}