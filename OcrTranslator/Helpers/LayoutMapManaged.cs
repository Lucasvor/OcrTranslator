using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Helpers;

public static class LayoutMapManaged
{
    private static Dictionary<uint, string> _keyMap = new Dictionary<uint, string>
        {
            // Letras (A-Z)
            { 65, "A" }, { 66, "B" }, { 67, "C" }, { 68, "D" }, { 69, "E" },
            { 70, "F" }, { 71, "G" }, { 72, "H" }, { 73, "I" }, { 74, "J" },
            { 75, "K" }, { 76, "L" }, { 77, "M" }, { 78, "N" }, { 79, "O" },
            { 80, "P" }, { 81, "Q" }, { 82, "R" }, { 83, "S" }, { 84, "T" },
            { 85, "U" }, { 86, "V" }, { 87, "W" }, { 88, "X" }, { 89, "Y" }, { 90, "Z" },

            // Números (0-9)
            { 48, "0" }, { 49, "1" }, { 50, "2" }, { 51, "3" }, { 52, "4" },
            { 53, "5" }, { 54, "6" }, { 55, "7" }, { 56, "8" }, { 57, "9" },

            // Teclas especiais
            { 8, "Backspace" },
            { 9, "Tab" },
            { 13, "Enter" },
            { 16, "Shift" },
            { 17, "Control" },
            { 18, "Alt" },
            { 20, "Caps Lock" },
            { 27, "Escape" },
            { 32, "Space" },
            { 33, "Page Up" },
            { 34, "Page Down" },
            { 35, "End" },
            { 36, "Home" },
            { 37, "Left Arrow" },
            { 38, "Up Arrow" },
            { 39, "Right Arrow" },
            { 40, "Down Arrow" },
            { 45, "Insert" },
            { 46, "Delete" },

            // Símbolos
            { 186, ";" }, { 187, "=" }, { 188, "," }, { 189, "-" }, { 190, "." },
            { 191, "/" }, { 192, "`" },
            { 219, "[" }, { 220, "\\" }, { 221, "]" }, { 222, "'" },

            // Números do teclado numérico
            { 96, "NumPad 0" }, { 97, "NumPad 1" }, { 98, "NumPad 2" }, { 99, "NumPad 3" },
            { 100, "NumPad 4" }, { 101, "NumPad 5" }, { 102, "NumPad 6" }, { 103, "NumPad 7" },
            { 104, "NumPad 8" }, { 105, "NumPad 9" },

            // Operadores do teclado numérico
            { 106, "Multiply" },
            { 107, "Add" },
            { 109, "Subtract" },
            { 110, "Decimal" },
            { 111, "Divide" },

            // Funções (F1-F12)
            { 112, "F1" }, { 113, "F2" }, { 114, "F3" }, { 115, "F4" },
            { 116, "F5" }, { 117, "F6" }, { 118, "F7" }, { 119, "F8" },
            { 120, "F9" }, { 121, "F10" }, { 122, "F11" }, { 123, "F12" }
        };
   
    public static string GetKeyName(uint key)
    {
        return _keyMap.TryGetValue(key, out var name) ? name : "Unknown Key";
    }

    public static uint GetKeyValue(string name)
    {
        foreach (var pair in _keyMap)
        {
            if (pair.Value.Equals(name, StringComparison.OrdinalIgnoreCase))
                return pair.Key;
        }

        throw new ArgumentException("Key name not found.", nameof(name));
    }
}
