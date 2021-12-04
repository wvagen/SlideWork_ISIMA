using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[CustomEditor(typeof(MS_SlideShow_Element))]
public class MS_SlideShow_Element_Editor : Editor
{

    string[] startAnimationList = new string[6] { "None", "Random", "Slide", "Scale", "Pop", "Lerp" };
    int selectedAnimationListIndex = 0;


    string[] loopingAnimationList = new string[5] { "None", "Random", "Pop", "Tilt", "Rotate" };
    string[] loopingAnimationRotationAxe = new string[3] { "Z", "Y", "X" };

    int selectedLoopableAnimationListIndex = 0;
    int selectedLoopableAniimationRotationIndex = 0;

    string[] exitAnimationList = new string[4] { "None", "Random", "Slide", "Shrink" };
    int selectedExitAnimationIndex = 0;

    string[] slideDirectionList = new string[6] { "Auto", "Random", "Right", "Left", "Up", "Down" };
    int selectedSlideDirectionIndex = 0;

    string[] animationPresenceBahaviorList = new string[3] { "Auto", "Parallel", "OnClick"};
    int selectedAnimationPresenceBahavior = 0;

    EditorCoroutine coroutine;
    public override void OnInspectorGUI()
    {
        MS_SlideShow_Element elementScript = (MS_SlideShow_Element)target;
        Start_Animation_GUI_Tools(elementScript);
        Loopable_Animation_GUI_Tools(elementScript);
        Exit_Animation_GUI_Tools(elementScript);
        Options_Animation_GUI_Tools(elementScript);
    }

    void Start_Animation_GUI_Tools(MS_SlideShow_Element elementScript)
    {
        GUILayout.Label("Start Behaivor", EditorStyles.boldLabel);
        Set_Init_Animation(elementScript);
        switch (elementScript.startAnimation)
        {
            case MS_SlideShow_Element.EnteringAnimation.Slide:
                Set_Init_Direction(elementScript);
                elementScript.startAnim_SlideSpeed = EditorGUILayout.Slider("Slide Speed:", elementScript.startAnim_SlideSpeed, 0.1f, 5f);
                break;
            case MS_SlideShow_Element.EnteringAnimation.Pop:
                elementScript.startAnim_PopSize = EditorGUILayout.Slider("Pop Max Size:", elementScript.startAnim_PopSize, -10f, 10f);
                elementScript.startAnim_ScaleSpeed = EditorGUILayout.Slider("Scale Speed :", elementScript.startAnim_ScaleSpeed, 1, 10f);
                break;
            case MS_SlideShow_Element.EnteringAnimation.Scale:
                {
                    elementScript.startAnim_ScaleSpeed = EditorGUILayout.Slider("Scale Speed :", elementScript.startAnim_ScaleSpeed, 1f, 10f);
                } break;
            case MS_SlideShow_Element.EnteringAnimation.Lerp:
                {
                    Set_Init_Direction(elementScript);
                    elementScript.startAnim_LerpSpeed = EditorGUILayout.Slider("Lerp Speed :", elementScript.startAnim_LerpSpeed, 0.1f, 5f);
                } break;
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Preview Animation"))
        {
            elementScript.Store_Init_Stats();
            elementScript.Prepare_Animation();
            coroutine = EditorCoroutineUtility.StartCoroutine(elementScript.Preview_Start_Animation(), this);
        }
        if (GUILayout.Button("Reset"))
        {
            if (coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
            }
            elementScript.Reset();
            Debug.Log("Reseted");
        }
        GUILayout.EndHorizontal();
    }

    void Set_Init_Animation(MS_SlideShow_Element elementScript)
    {
        this.selectedAnimationListIndex = EditorGUILayout.Popup("Start Animation", selectedAnimationListIndex, startAnimationList);
        switch (this.selectedAnimationListIndex)
        {
            case 0: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.None; break;
            case 1: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.Random; break;
            case 2: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.Slide; break;
            case 3: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.Scale; break;
            case 4: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.Pop; break;
            case 5: elementScript.startAnimation = MS_SlideShow_Element.EnteringAnimation.Lerp; break;
        }
    }

    void Set_Init_Direction(MS_SlideShow_Element elementScript)
    {
        this.selectedSlideDirectionIndex = EditorGUILayout.Popup("Direction", selectedSlideDirectionIndex, slideDirectionList);
        switch (this.selectedSlideDirectionIndex)
        {
            case 0: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Auto; break;
            case 1: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Random; break;
            case 2: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Right; break;
            case 3: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Left; break;
            case 4: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Up; break;
            case 5: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Down; break;
        }
    }

    void Set_Exit_Direction(MS_SlideShow_Element elementScript)
    {
        this.selectedSlideDirectionIndex = EditorGUILayout.Popup("Direction", selectedSlideDirectionIndex, slideDirectionList);
        switch (this.selectedSlideDirectionIndex)
        {
            case 0: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Auto; break;
            case 1: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Random; break;
            case 2: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Left; break;
            case 3: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Right; break;
            case 4: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Down; break;
            case 5: elementScript.slideDirection = MS_SlideShow_Element.AnimationSlideDirection.Up; break;
        }
    }

    void Loopable_Animation_GUI_Tools(MS_SlideShow_Element elementScript)
    {
        GUILayout.Space(20);
        GUILayout.Label("Looping Behaivor", EditorStyles.boldLabel);
        Set_Loopable_Animation(elementScript);

        switch (elementScript.loopableAnimation)
        {
            case MS_SlideShow_Element.LoopingAnimation.Pop:
                elementScript.loopableAnim_PopSize = EditorGUILayout.Slider("Pop Size:", elementScript.loopableAnim_PopSize, 0.1f, 5f);
                elementScript.loopableAnim_Speed = EditorGUILayout.Slider("Pop Speed:", elementScript.loopableAnim_Speed, 1f, 30f);
                break;
            case MS_SlideShow_Element.LoopingAnimation.Tilt:
                Set_Loopable_Rotation_Axe(elementScript);
                elementScript.loopableAnim_TiltDegree = EditorGUILayout.IntSlider("Tilt Degree:", elementScript.loopableAnim_TiltDegree, 1, 170);
                elementScript.loopableAnim_Speed = EditorGUILayout.Slider("Tilt Speed :", elementScript.loopableAnim_Speed, 1, 200);
                break;
            case MS_SlideShow_Element.LoopingAnimation.Rotation:
                {
                    Set_Loopable_Rotation_Axe(elementScript);
                    elementScript.loopableAnim_Speed = EditorGUILayout.Slider("Rotation Speed :", elementScript.loopableAnim_Speed, -200, 200);
                } break;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview Animation"))
        {
            elementScript.Store_Init_Stats();
            coroutine = EditorCoroutineUtility.StartCoroutine(elementScript.Preview_Loopable_Animation(), this);
        }
        if (GUILayout.Button("Reset"))
        {
            if (coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
            }
            elementScript.Reset();
            Debug.Log("Reseted");
        }
        GUILayout.EndHorizontal();
    }

    void Exit_Animation_GUI_Tools(MS_SlideShow_Element elementScript)
    {

        GUILayout.Space(20);
        GUILayout.Label("Exit Behaivor", EditorStyles.boldLabel);
        Set_Exit_Animation(elementScript);

        switch (elementScript.exitAnimation)
        {
            case MS_SlideShow_Element.ExitingAnimation.Slide:
                Set_Exit_Direction(elementScript);
                elementScript.exitAnim_SlideSpeed = EditorGUILayout.Slider("Slide Speed:", elementScript.exitAnim_SlideSpeed, 0.1f, 5f);
                break;
            case MS_SlideShow_Element.ExitingAnimation.Shrink:
                {
                   elementScript.exitAnim_ShrinkSpeed = EditorGUILayout.IntSlider("Shrink Speed :", elementScript.exitAnim_ShrinkSpeed, 1, 50);
                } break;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview Animation"))
        {
            elementScript.Store_Init_Stats();
            coroutine = EditorCoroutineUtility.StartCoroutine(elementScript.Preview_Exit_Animation(), this);
        }
        if (GUILayout.Button("Reset"))
        {
            if (coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
            }
            elementScript.Reset();
            Debug.Log("Reseted");
        }
        GUILayout.EndHorizontal();
    }

    void Options_Animation_GUI_Tools(MS_SlideShow_Element elementScript)
    {
        GUILayout.Space(20);
        GUILayout.Label("Options", EditorStyles.boldLabel);
        Set_Animation_Presence_Beahior(elementScript);
    }

    void Set_Animation_Presence_Beahior(MS_SlideShow_Element elementScript)
    {
        this.selectedAnimationPresenceBahavior = EditorGUILayout.Popup("Presence Behavior", selectedAnimationPresenceBahavior, animationPresenceBahaviorList);
        switch (this.selectedAnimationPresenceBahavior)
        {
            case 0: elementScript.animationOrder = MS_SlideShow_Element.AnimationLayering.Auto; break;
            case 1: elementScript.animationOrder = MS_SlideShow_Element.AnimationLayering.Parallel; break;
            case 2: elementScript.animationOrder = MS_SlideShow_Element.AnimationLayering.OnClick; break;
        }
    }

    void Set_Loopable_Animation(MS_SlideShow_Element elementScript)
    {
        this.selectedLoopableAnimationListIndex = EditorGUILayout.Popup("Looping Animation", selectedLoopableAnimationListIndex, loopingAnimationList);
        switch (this.selectedLoopableAnimationListIndex)
        {
            case 0: elementScript.loopableAnimation = MS_SlideShow_Element.LoopingAnimation.None; break;
            case 1: elementScript.loopableAnimation = MS_SlideShow_Element.LoopingAnimation.Random; break;
            case 2: elementScript.loopableAnimation = MS_SlideShow_Element.LoopingAnimation.Pop; break;
            case 3: elementScript.loopableAnimation = MS_SlideShow_Element.LoopingAnimation.Tilt; break;
            case 4: elementScript.loopableAnimation = MS_SlideShow_Element.LoopingAnimation.Rotation; break;
        }
    }

    void Set_Exit_Animation(MS_SlideShow_Element elementScript)
    {
        this.selectedExitAnimationIndex = EditorGUILayout.Popup("Exit Animation", selectedExitAnimationIndex, exitAnimationList);
        switch (this.selectedExitAnimationIndex)
        {
            case 0: elementScript.exitAnimation = MS_SlideShow_Element.ExitingAnimation.None; break;
            case 1: elementScript.exitAnimation = MS_SlideShow_Element.ExitingAnimation.Random; break;
            case 2: elementScript.exitAnimation = MS_SlideShow_Element.ExitingAnimation.Slide; break;
            case 3: elementScript.exitAnimation = MS_SlideShow_Element.ExitingAnimation.Shrink; break;
        }
    }

    void Set_Loopable_Rotation_Axe(MS_SlideShow_Element elementScript)
    {
        this.selectedLoopableAniimationRotationIndex = EditorGUILayout.Popup("Rotation Axe", selectedLoopableAniimationRotationIndex, loopingAnimationRotationAxe);
        switch (this.selectedLoopableAniimationRotationIndex)
        {
            case 0: elementScript.rotationAxe = MS_SlideShow_Element.RotationAxes.Z; break;
            case 1: elementScript.rotationAxe = MS_SlideShow_Element.RotationAxes.Y; break;
            case 2: elementScript.rotationAxe = MS_SlideShow_Element.RotationAxes.X; break;
        }
    }
}
