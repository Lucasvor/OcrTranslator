﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Settings;
public sealed class SettingItem<T> : INotifyPropertyChanged
{
    private T _value;

    public SettingItem(T startValue)
    {
        _value = startValue;
    }

    public T Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
            OnValueChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnValueChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }
}
