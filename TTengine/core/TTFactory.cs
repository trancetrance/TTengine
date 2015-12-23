﻿// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Artemis;
using TTengine.Comps;
using TTengine.Modifiers;
using TTMusicEngine.Soundevents;

namespace TTengine.Core
{
    /// <summary>
    /// The Singleton TTengine Factory to create new Entities (may be half-baked, 
    /// to further customize), and perhaps other things
    /// </summary>
    public sealed class TTFactory
    {
        /// <summary>The Artemis entity world currently used for building new Entities in</summary>
        public static EntityWorld BuildWorld;

        /// <summary>The screen that newly built Entities will render to.
        /// Value null is used to denote "default".</summary>
        public static ScreenComp BuildScreen;

        public static Channel BuildChannel
        {
            get
            {
                return new Channel(BuildWorld, BuildScreen);
            }
            set
            {
                BuildWorld = value.World;
                BuildScreen = value.Screen;
            }
        }

        private static TTGame _game = null;

        static TTFactory() {
            _game = TTGame.Instance;
        }

        public static void BuildTo(ScreenComp screen)
        {
            BuildScreen = screen;
        }

        /// <summary>
        /// Switch factory's building output to a new channel, new world or new screen
        /// </summary>
        /// <param name="e"></param>
        public static void BuildTo(Entity e)
        {
            if (e.HasComponent<WorldComp>())
                BuildWorld = e.GetComponent<WorldComp>().World;
            if (e.HasComponent<ScreenComp>())
                BuildScreen = e.GetComponent<ScreenComp>();
        }

        public static void BuildTo(Channel ch)
        {
            BuildWorld = ch.World;
            BuildScreen = ch.Screen;
        }

        /// <summary>
        /// Create simplest Entity without components within the EntityWorld currently selected
        /// for Entity construction
        /// </summary>
        /// <returns></returns>
        public static Entity CreateEntity()
        {
            return BuildWorld.CreateEntity();
        }

        /// <summary>
        /// Create a Gamelet, which is an Entity with position and velocity, but no shape/drawability (yet).
        /// </summary>
        /// <returns></returns>
        public static Entity CreateGamelet()
        {
            Entity e = CreateEntity();
            e.AddComponent(new PositionComp());
            e.AddComponent(new VelocityComp());
            return e;
        }

        /// <summary>
        /// Create a Drawlet, which is a moveable, drawable Entity
        /// </summary>
        /// <returns></returns>
        public static Entity CreateDrawlet()
        {
            Entity e = CreateGamelet();
            e.AddComponent(new DrawComp(BuildScreen));
            return e;
        }

        /// <summary>
        /// Create a Spritelet, which is a moveable sprite 
        /// </summary>
        /// <param name="graphicsFile">The content graphics file with or without extension. If
        /// extension given eg "ball.png", the uncompiled file will be loaded at runtime. If no extension
        /// given eg "ball", precompiled XNA content will be loaded (.xnb files).</param>
        /// <returns></returns>
        public static Entity CreateSpritelet(string graphicsFile)
        {
            Entity e = CreateDrawlet();
            var spriteComp = new SpriteComp(graphicsFile);
            e.AddComponent(spriteComp);
            return e;
        }

        /// <summary>
        /// Create a Spritelet with texture based on the contents of a Screenlet
        /// </summary>
        /// <returns></returns>
        public static Entity CreateSpritelet(Entity screen)
        {
            Entity e = CreateDrawlet();
            var spriteComp = new SpriteComp(screen.GetComponent<ScreenComp>());
            e.AddComponent(spriteComp);
            return e;
        }

        /// <summary>
        /// Create an animated sprite entity
        /// </summary>
        /// <param name="atlasBitmapFile">Filename of the sprite atlas bitmap</param>
        /// <param name="NspritesX">Number of sprites in horizontal direction (X) in the atlas</param>
        /// <param name="NspritesY">Number of sprites in vertical direction (Y) in the atlas</param>
        /// <returns></returns>
        public static Entity CreateAnimatedSpritelet(string atlasBitmapFile, int NspritesX, int NspritesY)
        {
            return CreateAnimatedSpritelet(atlasBitmapFile, NspritesX, NspritesY, AnimationType.NORMAL);
        }

            /// <summary>
        /// Create an animated sprite entity
        /// </summary>
        /// <param name="atlasBitmapFile">Filename of the sprite atlas bitmap</param>
        /// <param name="NspritesX">Number of sprites in horizontal direction (X) in the atlas</param>
        /// <param name="NspritesY">Number of sprites in vertical direction (Y) in the atlas</param>
        /// <param name="animType">Animation type chosen from AnimationType class</param>
        /// <returns></returns>
        public static Entity CreateAnimatedSpritelet(string atlasBitmapFile, int NspritesX, int NspritesY, AnimationType animType)
        {
            Entity e = CreateDrawlet();
            var spriteComp = new AnimatedSpriteComp(atlasBitmapFile,NspritesX,NspritesY);
            spriteComp.AnimType = animType;
            e.AddComponent(spriteComp);
            return e;
        }

        public static Entity CreateSpriteField(string fieldBitmapFile, string spriteBitmapFile)
        {
            Entity e = CreateDrawlet();
            var spriteFieldComp = new SpriteFieldComp(fieldBitmapFile);
            var spriteComp = new SpriteComp(spriteBitmapFile);
            spriteFieldComp.FieldSpacing = new Vector2(spriteComp.Width, spriteComp.Height);
            e.AddComponent(spriteComp);
            e.AddComponent(spriteFieldComp);
            return e;
        }

        /// <summary>
        /// Creates a Textlet, which is a moveable piece of text.
        /// Uses a default font.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Entity CreateTextlet(string text)
        {
            return CreateTextlet(text, "Font1");
        }

        /// <summary>
        /// Creates a Textlet, which is a moveable piece of text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static Entity CreateTextlet(string text, string fontName)
        {
            Entity e = CreateDrawlet();
            e.AddComponent(new ScaleComp());
            TextComp tc = new TextComp(text, fontName);
            e.AddComponent(tc);
            return e;
        }

        /// <summary>
        /// Creates a Screenlet that may contain a screenComp (RenderBuffer) to 
        /// which graphics can be rendered. 
        /// </summary>
        /// <param name="hasRenderBuffer">if true, Screenlet will have its own render buffer</param>
        /// <param name="height">Screenlet height, if not given uses default backbuffer height</param>
        /// <param name="width">Screenlet width, if not given uses default backbuffer width</param>
        /// <returns></returns>
        public static Entity CreateScreenlet(bool hasRenderBuffer=false,
                                        int width = 0, int height = 0)
        {
            var sc = new ScreenComp(hasRenderBuffer, width, height);
            var e = CreateEntity();
            e.AddComponent(sc);
            e.AddComponent(new DrawComp(BuildScreen));
            return e;
        }

        /// <summary>
        /// Creates a Screenlet that may contain a screenComp (RenderBuffer) to 
        /// which graphics can be rendered. 
        /// </summary>
        /// <param name="backgroundColor">Background color of the Screenlet</param>
        /// <returns></returns>
        public static Entity CreateScreenlet(Color backgroundColor, bool hasRenderBuffer = false,
                                        int width = 0, int height = 0)
        {
            var e = CreateScreenlet(hasRenderBuffer, width, height);
            e.GetComponent<ScreenComp>().BackgroundColor = backgroundColor;
            return e;
        }

        public static Entity CreateChannel(Color backgroundColor, bool hasRenderBuffer = false,
                                        int width = 0, int height = 0)
        {
            var e = CreateScreenlet(backgroundColor, hasRenderBuffer, width, height);
            var wc = new WorldComp();
            e.AddComponent(wc);
            BuildTo(e);
            return e;
        }

        /// <summary>
        /// Creates an FX Screenlet that renders a layer with shader Effect to the current active BuildScreen
        /// </summary>
        /// <returns></returns>
        public static Entity CreateFxScreenlet(String effectFile)
        {
            var fx = TTGame.Instance.Content.Load<Effect>(effectFile);
            var sc = new ScreenComp(BuildScreen.RenderTarget); // renders to the existing screen buffer
            sc.SpriteBatch.effect = fx; // set the effect in SprBatch
            var e = CreateEntity();
            e.AddComponent(sc);
            e.AddComponent(new DrawComp(sc));
            return e;
        }

        /// <summary>
        /// Creates a Scriptlet, which is an Entity that only contains a custom code script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Entity CreateScriptlet(IScript script)
        {
            var e = CreateEntity();
            e.AddComponent(new ScriptComp(script));
            e.Refresh();
            return e;
        }

        /// <summary>
        /// Create an Audiolet, which is an Entity that only contains an audio script
        /// </summary>
        /// <param name="soundScript"></param>
        /// <returns></returns>
        public static Entity CreateAudiolet(SoundEvent soundScript)
        {
            var e = CreateEntity();
            e.AddComponent(new AudioComp(soundScript));
            e.Refresh();
            return e;
        }

        public static void AddScript(Entity e, IScript script)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            var sc = e.GetComponent<ScriptComp>();
            sc.Add(script);
        }

        /// <summary>
        /// Add a script to an Entity, based on a function (delegate)
        /// </summary>
        /// <param name="e">The Entity to add script to</param>
        /// <param name="scriptCode">Script to run</param>
        /// <returns>IScript object created from the function</returns>
        public static BasicScript AddScript(Entity e, ScriptDelegate scriptCode)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            var sc = e.GetComponent<ScriptComp>();
            var script = new BasicScript(scriptCode);
            sc.Add(script);
            return script;
        }

        /// <summary>
        /// Add a Modifier script to an Entity, based on a code block (delegate) and a Function
        /// </summary>
        /// <param name="e">Entity to add modifier script to</param>
        /// <param name="scriptCode">Code block (delegate) that is the script</param>
        /// <param name="func">Function whose value will be passed in ScriptContext.FunctionValue to script</param>
        /// <returns></returns>
        public static ModifierScript AddModifier(Entity e, ModifierDelegate scriptCode, IFunction func)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            var sc = e.GetComponent<ScriptComp>();
            var script = new ModifierScript(scriptCode, func);
            sc.Add(script);
            return script;
        }

        /// <summary>
        /// Add a Modifier script to an Entity, based on a code block (delegate) and a VectorFunction
        /// </summary>
        /// <param name="e">Entity to add modifier script to</param>
        /// <param name="scriptCode">Code block (delegate) that is the script</param>
        /// <param name="func">Function whose value will be passed in ScriptContext.FunctionValue to script</param>
        /// <returns></returns>
        public static VectorModifierScript AddModifier(Entity e, VectorModifierDelegate scriptCode, IVectorFunction func)
        {
            if (!e.HasComponent<ScriptComp>())
                e.AddComponent(new ScriptComp());
            var sc = e.GetComponent<ScriptComp>();
            var script = new VectorModifierScript(scriptCode, func);
            sc.Add(script);
            return script;
        }

        /// <summary>
        /// Add a Modifier script to an Entity, based on a code block (delegate) and an empty (=unity) Function
        /// </summary>
        /// <param name="e">Entity to add modifier script to</param>
        /// <param name="scriptCode">Code block (delegate) that is the script</param>
        /// <returns></returns>
        public static ModifierScript AddModifier(Entity e, ModifierDelegate scriptCode)
        {
            return AddModifier(e, scriptCode, null);
        }

        /// <summary>
        /// Basic script object that can run code from a Delegate
        /// </summary>
        public class BasicScript : IScript
        {
            protected ScriptDelegate scriptCode;

            public BasicScript(ScriptDelegate scriptCode)
            {
                this.scriptCode = scriptCode;
            }

            public void OnUpdate(ScriptContext ctx)
            {
                scriptCode(ctx);
            }

        }

    }
}
