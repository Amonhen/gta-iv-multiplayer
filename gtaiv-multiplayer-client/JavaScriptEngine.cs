﻿// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using Jint;
using System.Drawing;

namespace MIVClient
{
    public class JSAPI
    {
        public ClientTextView createTextBoxDraw(float x, float y, float width, float height, byte alignment, string text, GTA.Font font, int color, byte alpha)
        {
            return new ClientTextView(new RectangleF(x, y, width, height), (GTA.TextAlignment)alignment, text, font, Color.FromArgb(alpha, Color.FromArgb(color)));
        }
        public ClientTextView createTextDraw(float x, float y, string text, GTA.Font font, int color, byte alpha)
        {
            return new ClientTextView(new PointF(x, y), text, font, Color.FromArgb(alpha, Color.FromArgb(color)));
        }
        public GTA.Font createFont(string name, float size)
        {
            return new GTA.Font(name, size, GTA.FontScaling.Pixel);
        }
        public InterpolatorBase createInterpolator(string name, float start, float end, int duration)
        {
            if (name == "linear") return new LinearInterpolator(start, end, duration);
            if (name == "easeinout") return new EaseInOutInterpolator(start, end, duration);
            else return null;
        }
        public void writeChat(string text)
        {
            Client.instance.chatController.writeChat(text);
        }
        public System.Timers.Timer setTimer(int i, Delegate d)
        {
            var timer = new System.Timers.Timer(i);
            timer.Elapsed += (o, e) => d.DynamicInvoke();
            timer.Start();
            timer.Enabled = true;
            return timer;
        }
    }
    
    public class JavaScriptEngine
    {
        Engine engine;

        public JavaScriptEngine()
        {
            try
            {
                engine = new Engine(c => c.Strict(true));
                engine.SetValue("Client", Client.instance);
                engine.SetValue("API", new JSAPI());
                engine.SetValue("vec3", new Func<float, float, float, GTA.Vector3>((x, y, z) => { return new GTA.Vector3(x, y, z); }));
            }
            catch (Exception e)
            {
                GTA.Game.Log(e.Message);
            }
              
        }

        public void Execute(string script)
        {
            try
            {
                engine.Execute(script);
            }
            catch (Exception e)
            {
                GTA.Game.Log(e.Message);
            }
        }

        public T Execute<T>(string script)
        {
            return (T)engine.Execute(script).GetCompletionValue().ToObject();
        }
    }
}
