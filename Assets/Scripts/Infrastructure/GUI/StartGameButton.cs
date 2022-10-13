using System;
using UnityEngine;

namespace Infrastructure.GUI
{
    public sealed class StartGameButton
    {
        private readonly Action _onClick;

        public StartGameButton(Action onClick) =>
            _onClick = onClick;

        public void Draw()
        {
            if (UnityEngine.GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
                _onClick?.Invoke();
        }
    }
}
