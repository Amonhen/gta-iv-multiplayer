﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using GTA;
using System.Drawing;

namespace MIVClient
{
    public class PerFrameRenderer
    {
        Client client;
        GTA.Font font;
        RectangleF chatPosition;
        Color chatBackground;
        public PerFrameRenderer(Client client)
        {
            this.client = client;
            chatPosition = new RectangleF(10, 10, 400, 200);
            chatBackground = Color.FromArgb(70, 0, 0, 0);
            font = new GTA.Font("Consolas", 24, FontScaling.Pixel);
            client.PerFrameDrawing += Client_PerFrameDrawing;
        }

        void Client_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            e.Graphics.Scaling = FontScaling.Pixel;
            e.Graphics.DrawRectangle(chatPosition, chatBackground);
            int yoffset = 15;
            for (int i = 0; i < client.chatController.chatconsole.Count; i++ )
            {
                e.Graphics.DrawText(client.chatController.chatconsole.ToArray()[i], 15, yoffset, font);
                yoffset += 30;
            }
            if (client.keyboardHandler.inKeyboardTypingMode)
            {
                int cpos = client.keyboardHandler.cursorpos;
                string prefix = client.nick + ": ";
                string currenttext = prefix + client.chatController.currentTypedText;
                
                e.Graphics.DrawText(currenttext, 25, 15 + 30 * 8 + 20, font);
                if (DateTime.Now.Millisecond < 500)
                {
                    StringBuilder cstr = new StringBuilder();
                    cstr.Append(' ', cpos + prefix.Length); 
                    cstr.Append('|');
                    e.Graphics.DrawText(cstr.ToString(), 25, 15 + 30 * 8 + 20, font);
                }
            }

        }
    }
}