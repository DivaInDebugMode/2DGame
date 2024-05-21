using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class KeyBoardConverterToImage : MonoBehaviour
    {
        [SerializeField] private List<Sprite> keyBoard = new();
        [SerializeField] private Sprite noneSprite;

        public Sprite GetSpriteForPath(string path) => path switch
        {
            // Arrow keys
            "<Keyboard>/upArrow"   => keyBoard[0],
            "<Keyboard>/downArrow" => keyBoard[1],
            "<Keyboard>/leftArrow" => keyBoard[2],
            "<Keyboard>/rightArrow"=> keyBoard[3],
            
            // Alphabet keys
            "<Keyboard>/a" => keyBoard[4],
            "<Keyboard>/b" => keyBoard[5],
            "<Keyboard>/c" => keyBoard[6],
            "<Keyboard>/d" => keyBoard[7],
            "<Keyboard>/e" => keyBoard[8],
            "<Keyboard>/f" => keyBoard[9],
            "<Keyboard>/g" => keyBoard[10],
            "<Keyboard>/h" => keyBoard[11],
            "<Keyboard>/i" => keyBoard[12],
            "<Keyboard>/j" => keyBoard[13],
            "<Keyboard>/k" => keyBoard[14],
            "<Keyboard>/l" => keyBoard[15],
            "<Keyboard>/m" => keyBoard[16],
            "<Keyboard>/n" => keyBoard[17],
            "<Keyboard>/o" => keyBoard[18],
            "<Keyboard>/p" => keyBoard[19],
            "<Keyboard>/q" => keyBoard[20],
            "<Keyboard>/r" => keyBoard[21],
            "<Keyboard>/s" => keyBoard[22],
            "<Keyboard>/t" => keyBoard[23],
            "<Keyboard>/u" => keyBoard[24],
            "<Keyboard>/v" => keyBoard[25],
            "<Keyboard>/w" => keyBoard[26],
            "<Keyboard>/x" => keyBoard[27],
            "<Keyboard>/y" => keyBoard[28],
            "<Keyboard>/z" => keyBoard[29],
            
            // Number keys
            "<Keyboard>/0" => keyBoard[30],
            "<Keyboard>/1" => keyBoard[31],
            "<Keyboard>/2" => keyBoard[32],
            "<Keyboard>/3" => keyBoard[33],
            "<Keyboard>/4" => keyBoard[34],
            "<Keyboard>/5" => keyBoard[35],
            "<Keyboard>/6" => keyBoard[36],
            "<Keyboard>/7" => keyBoard[37],
            "<Keyboard>/8" => keyBoard[38],
            "<Keyboard>/9" => keyBoard[39],
            
            // Function keys
            "<Keyboard>/f1" => keyBoard[40],
            "<Keyboard>/f2" => keyBoard[41],
            "<Keyboard>/f3" => keyBoard[42],
            "<Keyboard>/f4" => keyBoard[43],
            "<Keyboard>/f5" => keyBoard[44],
            "<Keyboard>/f6" => keyBoard[45],
            "<Keyboard>/f7" => keyBoard[46],
            "<Keyboard>/f8" => keyBoard[47],
            "<Keyboard>/f9" => keyBoard[48],
            "<Keyboard>/f10" => keyBoard[49],
            "<Keyboard>/f11" => keyBoard[50],
            "<Keyboard>/f12" => keyBoard[51],

            // Other keys
            "<Keyboard>/leftShift" => keyBoard[52],
            "<Keyboard>/rightShift" => keyBoard[53],
            "<Keyboard>/rightAlt" => keyBoard[54],
            "<Keyboard>/leftAlt" => keyBoard[55],
            "<Keyboard>/tab" => keyBoard[56],
            "<Keyboard>/leftCtrl" => keyBoard[57],
            "<Keyboard>/rightCtrl" => keyBoard[58],
            "<Keyboard>/rightBracket" => keyBoard[59],
            "<Keyboard>/leftBracket" => keyBoard[60],
            "<Keyboard>/pageUp" => keyBoard[61],
            "<Keyboard>/pageDown" => keyBoard[62],
            "<Keyboard>/comma" => keyBoard[63],
            "<Keyboard>/period" => keyBoard[64],
           "<Keyboard>/backslash" => keyBoard[65],
            "<Keyboard>/slash" => keyBoard[66],
            "<Keyboard>/semicolon" => keyBoard[67],
            "<Keyboard>/quote" => keyBoard[68],
            "<Keyboard>/minus" => keyBoard[69],
            "<Keyboard>/equals" => keyBoard[70],
            "<Keyboard>/backspace" => keyBoard[71],
            "<Keyboard>/end" => keyBoard[72],
            "<Keyboard>/delete" => keyBoard[73],
            "<Keyboard>/home" => keyBoard[74],
            "<Keyboard>/space" => keyBoard[75],
            "<Keyboard>/backquote" => keyBoard[76],
            
            // NumPad keys
            "<Keyboard>/numpad0" => keyBoard[77],
            "<Keyboard>/numpad1" => keyBoard[78],
            "<Keyboard>/numpad2" => keyBoard[79],
            "<Keyboard>/numpad3" => keyBoard[80],
            "<Keyboard>/numpad4" => keyBoard[81],
            "<Keyboard>/numpad5" => keyBoard[82],
            "<Keyboard>/numpad6" => keyBoard[83],
            "<Keyboard>/numpad7" => keyBoard[84],
            "<Keyboard>/numpad8" => keyBoard[85],
            "<Keyboard>/numpad9" => keyBoard[86],
            "<Keyboard>/numpadDivide" => keyBoard[87],
            "<Keyboard>/numpadMinus" => keyBoard[88],
            "<Keyboard>/numpadPlus" => keyBoard[89],
            "<Keyboard>/numpadMultiply" => keyBoard[90],
            "<Keyboard>/numLock" => keyBoard[91],
            "<Keyboard>/numpadPeriod" => keyBoard[92],
            
            //...None..//
            _ => noneSprite


        };
    }
}