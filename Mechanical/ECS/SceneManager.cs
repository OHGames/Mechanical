//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Mechanical
//{
//    /// <summary>
//    /// The scene manager is a static class that handles scene transitions, loading, drawing, and updating.
//    /// </summary>
//    public static class SceneManager
//    {

//        /// <summary>
//        /// The current scene is the scene that is being updated, drawn, and active.
//        /// </summary>
//        public static Scene CurrentScene { get; private set; }

//        /// <summary>
//        /// This is the scene's startup entities. When the scene needs to be reset, it destroys all entities and remakes them from this list.
//        /// </summary>
//        private static EntityList currentScenesStartupEntities = new EntityList();

//        /// <summary>
//        /// This variable is set when there needs to be a scene transition.
//        /// </summary>
//        private static Scene toScene = null;

//        /// <summary>
//        /// All the scenes that are loaded.
//        /// </summary>
//        public static Dictionary<string, Scene> LoadedScenes = new Dictionary<string, Scene>();

//        /// <summary>
//        /// The current scene's transition.
//        /// </summary>
//        private static SceneTransition currentSceneTransition;

//        /// <summary>
//        /// The next scene's transition.
//        /// </summary>
//        private static SceneTransition nextSceneTransition;

//        /// <summary>
//        /// The transition to draw.
//        /// </summary>
//        private static SceneTransition transitionToDraw;

//        /// <summary>
//        /// If the manager is transitioning.
//        /// </summary>
//        public static bool IsTransitioning { get; }

//        public static void Initialize()
//        {
//            CurrentScene.Initialize();
//        }

//        public static void LoadContent(ContentManager content)
//        {
//            CurrentScene.LoadContent(content);
//        }

//        public static void Update(float deltaTime)
//        {

//            if (toScene != null)
//            {
//                // transition scenes.
//                DoSceneTransition();
//            }

//            CurrentScene.Update(deltaTime);

//            if (transitionToDraw != null)
//            {
//                transitionToDraw.Update(deltaTime);
//            }
//        }

//        public static void Draw()
//        {
//            CurrentScene.Draw();

//            // draw the scene transition.
//            if (transitionToDraw != null)
//            {
//                transitionToDraw.Draw();
//            }
//        }

//        /// <summary>
//        /// This function is the function that actually transitions the scene.
//        /// </summary>
//        private static void DoSceneTransition()
//        {
//            currentSceneTransition.Initialize();
//            currentSceneTransition.LoadContent(Engine.Instance.Content);

//            currentSceneTransition.Start(CurrentScene.RenderTarget);
//            currentSceneTransition.OnComplete += CurrentTransitionComplete;
//        }

//        private static void CurrentTransitionComplete()
//        {
//            // unhook
//            currentSceneTransition.OnComplete -= CurrentTransitionComplete;

//            // set new current scene.
//            CurrentScene.IsActiveScene = false;
//            toScene.IsActiveScene = true;

//            // start the transition.
//            transitionToDraw = nextSceneTransition;

//            // set up
//            nextSceneTransition.Initialize();
//            nextSceneTransition.LoadContent(Engine.Instance.Content);

//            // start transition.
//            nextSceneTransition.Start(toScene.RenderTarget);

//            // change scenes.
//            CurrentScene = toScene;
//            toScene = null;

//            // set entities.
//            currentScenesStartupEntities = new EntityList
//            {
//                currentScenesStartupEntities.ToArray()
//            };
//        }


//        /// <summary>
//        /// Call this function when you want to transition to a scene.
//        /// </summary>
//        /// <param name="name">The name of the scene.</param>
//        /// <param name="loadFromFile">If the scene should be loaded from a file. By default, it is false because it is recomended to laod the scenes when the game first starts.</param>
//        /// <param name="nextTransition">The transition to play when the next scene is loaded.</param>
//        /// <param name="transition">The transition to play as the scene comes to an end.</param>
//        public static void TransitionToScene(string name, SceneTransition transition, SceneTransition nextTransition, bool loadFromFile = false)
//        {
//            if (transition == null) currentSceneTransition = new NoTransition(TransitionType.Out, new TimeSpan(), Color.Transparent);
//            else currentSceneTransition = transition;

//            if (nextTransition == null) nextSceneTransition = new NoTransition(TransitionType.In, new TimeSpan(), Color.Transparent);
//            else nextSceneTransition = nextTransition;

//            transitionToDraw = currentSceneTransition;

//            if (!loadFromFile)
//            {
//                if (!LoadedScenes.ContainsKey(name)) throw new Exception($"The scene, {name}, does not exsist.");

//                toScene = LoadedScenes[name];
//            }
//            else
//            {
//                // todo.
//            }
//        }

//        /// <summary>
//        /// This will pause the <see cref="CurrentScene"/>
//        /// </summary>
//        public static void Pause()
//        {
//            CurrentScene.Paused = true;
//        }

//        /// <summary>
//        /// Reset the current scene's entities.
//        /// </summary>
//        /// <param name="doTransition">If there should be a transition.</param>
//        /// <param name="to">The transition to start.</param>
//        /// <param name="after">The transition to end.</param>
//        public static void ResetScene(bool doTransition, SceneTransition to = null, SceneTransition after = null)
//        {
//            if (to == null) currentSceneTransition = new NoTransition(TransitionType.Out, new TimeSpan(), Color.Transparent);
//            else currentSceneTransition = to;

//            if (after == null) nextSceneTransition = new NoTransition(TransitionType.In, new TimeSpan(), Color.Transparent);
//            else nextSceneTransition = after;

//            if (!doTransition)
//            {
//                CurrentScene.Entities = new EntityList() { currentScenesStartupEntities.ToArray() };
//            }
//            else
//            {
//                TransitionToScene(CurrentScene.Name, to, after);
//                to.OnComplete += () => 
//                {
                    
//                    CurrentScene.Entities = new EntityList() { currentScenesStartupEntities.ToArray() };

//                };
//            }
//        }

//        public static void GraphicsDeviceCreated(object sender, System.EventArgs e)
//        {
//            CurrentScene.OnGraphicsDeviceCreated(sender, e);
//        }

//        public static void GraphicsDeviceReset(object sender, System.EventArgs e)
//        {
//            CurrentScene.OnGraphicsDeviceReset(sender, e);
//        }

//        /// <summary>
//        /// Sets the startup scene
//        /// </summary>
//        /// <param name="scene">The scene.</param>
//        public static void SetStartupScene(Scene scene)
//        {
//            CurrentScene = scene;
//            currentScenesStartupEntities = new EntityList() { scene.Entities.ToArray() };
//            scene.IsActiveScene = true;
//        }

//    }
//}
