using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;

#endif

public class MS_SlideShow_Element : MonoBehaviour
{
    //Start Animation Fields
    public enum EnteringAnimation { None, Random, Slide, Scale, Pop, Lerp };
    public EnteringAnimation startAnimation;
    public float startAnim_SlideSpeed = 1.5f;

    public float startAnim_ScaleSpeed = 1f;
    public float startAnim_PopSize = 1.2f;
    public float startAnim_LerpSpeed = 1.2f;

    public bool isStartingAnimationEnds = false;
    bool isStartingAnimating = false;
    //End of Start Animation Fields


    //Loopable Animation Fields
    public enum LoopingAnimation { None, Random, Pop, Tilt, Rotation };

    public LoopingAnimation loopableAnimation;

    public enum RotationAxes { X, Y, Z };

    public RotationAxes rotationAxe;

    public float loopableAnim_PopSize = 1.2f;
    public int loopableAnim_TiltDegree = 15;

    public float loopableAnim_Speed = 1.2f;

    bool isLooping = false;
    //End of Loopable Animation Fields


    //Exit Animation Fields
    public enum ExitingAnimation { None, Random, Slide, Shrink };
    public ExitingAnimation exitAnimation;

    public float exitAnim_SlideSpeed = 1.5f;

    public int exitAnim_ShrinkSpeed = 15;

    //End of Exit Animation Fields

    public enum AnimationSlideDirection { Auto, Random, Right, Left, Up, Down };
    public AnimationSlideDirection slideDirection;

    //Option Fields
    public enum AnimationLayering { Auto, Parallel, OnClick};
    public AnimationLayering animationOrder;

    //End of Option Fields

    Vector3 initPos, initScale, initRot;
    bool isStatsInitted = false;

    void OnEnable()
    {
        startAnimation = EnteringAnimation.Slide;
        animationOrder = AnimationLayering.Parallel;



        startAnim_SlideSpeed = 1.5f;
        isStartingAnimating = false;
    }

    public void Store_Init_Stats()
    {
        if (!isStatsInitted)
        {
            initPos = transform.position;
            initScale = transform.localScale;
            initRot = transform.rotation.eulerAngles;
            isStatsInitted = true;
        }
    }

    public void Prepare_Animation()
    {
        transform.position = initPos;
        transform.localScale = initScale;
        transform.rotation = Quaternion.Euler(initRot);

        if (startAnimation == EnteringAnimation.Random)
        {
            switch (Random.Range(0, 4))
            {
                case 0: startAnimation = EnteringAnimation.Scale; break;
                case 1: startAnimation = EnteringAnimation.Slide; break;
                case 2: startAnimation = EnteringAnimation.Pop; break;
                case 3: startAnimation = EnteringAnimation.Lerp; break;
            }
        }

        switch (startAnimation)
        {
            case EnteringAnimation.Scale:
                transform.localScale = Vector2.zero; break;
            case EnteringAnimation.Slide:
                transform.position = Position_Related_To_Screen(transform.position);break;
            case EnteringAnimation.Lerp:
                transform.position = Position_Related_To_Screen(transform.position);break;
            case EnteringAnimation.Pop:
                transform.localScale = initScale;break;
        }

    }

    public IEnumerator Preview_Start_Animation()
    {
        if (!isStartingAnimating)
        {
            isStartingAnimating = true;
            isStartingAnimationEnds = false;

            if (startAnimation == EnteringAnimation.Random)
            {
                switch (Random.Range(0, 4))
                {
                    case 0: startAnimation = EnteringAnimation.Scale; break;
                    case 1: startAnimation = EnteringAnimation.Slide; break;
                    case 2: startAnimation = EnteringAnimation.Pop; break;
                    case 3: startAnimation = EnteringAnimation.Lerp; break;
                }
            }

            switch (startAnimation)
            {
                case EnteringAnimation.Scale:

                    while (Vector2.Distance(transform.localScale, initScale) > 0.1f)
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, initScale, Time.deltaTime * startAnim_ScaleSpeed);
                        yield return null;
                    }

                    transform.localScale = initScale;
                    isStartingAnimating = false;
                    isStartingAnimationEnds = true;
                    break;

                case EnteringAnimation.Slide:

                    while (Vector2.Distance(transform.position, initPos) > 0.1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, initPos, Time.deltaTime * startAnim_SlideSpeed * 1000);
                        yield return new WaitForEndOfFrame();
                    }

                    transform.position = initPos;
                    isStartingAnimating = false;
                    isStartingAnimationEnds = true;
                    break;


                case EnteringAnimation.Lerp:

                    while (Vector2.Distance(transform.localScale, initPos) > 0.1f)
                    {
                        transform.position = Vector3.Lerp(transform.position, initPos, Time.deltaTime * startAnim_LerpSpeed);
                        yield return null;
                    }

                    transform.localScale = initPos;
                    isStartingAnimating = false;
                    isStartingAnimationEnds = true;
                    break;

                case EnteringAnimation.Pop:

                    Vector2 targetScalePop = initScale * startAnim_PopSize;

                    while (Vector2.Distance(transform.localScale, targetScalePop) > 0.1f)
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, targetScalePop, Time.deltaTime * startAnim_ScaleSpeed);
                        yield return new WaitForEndOfFrame();
                    }
                    while (Vector2.Distance(transform.localScale, initScale) > 0.1f)
                    {
                        transform.localScale = Vector3.Lerp(transform.localScale, initScale, Time.deltaTime * startAnim_ScaleSpeed);
                        yield return new WaitForEndOfFrame();
                    }

                    transform.localScale = initScale;
                    isStartingAnimating = false;
                    isStartingAnimationEnds = true;
                    break;
            }
        }
    }

    public IEnumerator Preview_Loopable_Animation()
    {
        if (!isStartingAnimating)
        {
            isLooping = true;

            if (loopableAnimation == LoopingAnimation.Random)
            {
                switch (Random.Range(0, 3))
                {
                    case 0: loopableAnimation = LoopingAnimation.Pop; break;
                    case 1: loopableAnimation = LoopingAnimation.Rotation; break;
                    case 2: loopableAnimation = LoopingAnimation.Tilt; break;
                }
            }

            switch (loopableAnimation)
            {
                case LoopingAnimation.Pop:
                    {
                        float animation;
                        while (isLooping)
                        {
                            animation = loopableAnim_PopSize + Mathf.Sin(Time.time * loopableAnim_Speed) * 1 / 7;
                            transform.localScale = Vector3.one * animation;
                            yield return null;
                        }
                    } break;

                case LoopingAnimation.Tilt:
                    {
                        switch (rotationAxe)
                        {
                            case RotationAxes.Z:
                                {
                                    while (isLooping)
                                    {
                                        while (transform.eulerAngles.z < 180 || transform.eulerAngles.z > (360 - loopableAnim_TiltDegree))
                                        {
                                            transform.Rotate(-Vector3.forward * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        while (transform.eulerAngles.z < loopableAnim_TiltDegree || transform.eulerAngles.z > 180)
                                        {
                                            transform.Rotate(Vector3.forward * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        yield return null;
                                    }
                                } break;
                            case RotationAxes.Y:
                                {
                                    while (isLooping)
                                    {
                                        while (transform.eulerAngles.y < 180 || transform.eulerAngles.y > (360 - loopableAnim_TiltDegree))
                                        {
                                            transform.Rotate(-Vector3.up * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        while (transform.eulerAngles.y < loopableAnim_TiltDegree || transform.eulerAngles.y > 180)
                                        {
                                            transform.Rotate(Vector3.up * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        yield return null;
                                    }
                                } break;
                            case RotationAxes.X:
                                {
                                    while (isLooping)
                                    {
                                        while (transform.eulerAngles.x < 180 || transform.eulerAngles.x > (360 - loopableAnim_TiltDegree))
                                        {
                                            transform.Rotate(-Vector3.right * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        while (transform.eulerAngles.x < loopableAnim_TiltDegree || transform.eulerAngles.x > 180)
                                        {
                                            transform.Rotate(Vector3.right * Time.deltaTime * loopableAnim_Speed);
                                            yield return null;
                                        }
                                        yield return null;
                                    }
                                } break;
                        }
                    } break;
                case LoopingAnimation.Rotation:
                    {
                        switch (rotationAxe)
                        {
                            case RotationAxes.Z:
                                {
                                    while (isLooping)
                                    {
                                        transform.Rotate(-Vector3.forward * Time.deltaTime * loopableAnim_Speed);
                                        yield return null;
                                    }
                                } break;
                            case RotationAxes.Y:
                                {
                                    while (isLooping)
                                    {
                                        transform.Rotate(-Vector3.up * Time.deltaTime * loopableAnim_Speed);
                                        yield return null;
                                    }
                                } break;
                            case RotationAxes.X:
                                {
                                    while (isLooping)
                                    {
                                        transform.Rotate(-Vector3.right * Time.deltaTime * loopableAnim_Speed);
                                        yield return null;
                                    }
                                } break;
                        }
                    } break;

            }
        }
    }

    public IEnumerator Preview_Exit_Animation()
    {
        if (exitAnimation == ExitingAnimation.Random)
        {
            switch (Random.Range(0, 3))
            {
                case 0: exitAnimation = ExitingAnimation.Shrink; break;
                case 1: exitAnimation = ExitingAnimation.Slide; break;
            }
        }
        switch (exitAnimation)
        {
            case ExitingAnimation.Shrink:
                {
                    Vector2 currentScale = transform.localScale;
                    while (transform.localScale.x > 0)
                    {
                        currentScale -= Vector2.one * Time.deltaTime * exitAnim_ShrinkSpeed;
                        yield return null;
                    }
                } break;

            case ExitingAnimation.Slide:
                {
                    Vector2 targetPos = Position_Related_To_Screen(transform.position);

                    while (Vector2.Distance(transform.position, targetPos) > 0.1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * exitAnim_SlideSpeed * 1000);
                        yield return null;
                    }

                    transform.position = targetPos;
                }
                break;
        }

    }

    public void Reset()
    {
        transform.position = initPos;
        transform.localScale = initScale;
        transform.eulerAngles = initRot;
        isStartingAnimating = false;
        isLooping = false;
    }

    Vector2 Position_Related_To_Screen(Vector2 position)
    {
        Vector2 myUpdatedPosition = position;

        switch (slideDirection)
        {
            case AnimationSlideDirection.Down: myUpdatedPosition.y += Screen.height; break;
            case AnimationSlideDirection.Up: myUpdatedPosition.y -= Screen.height; break;
            case AnimationSlideDirection.Right: myUpdatedPosition.x -= Screen.width; break;
            case AnimationSlideDirection.Left: myUpdatedPosition.x += Screen.width; break;
            case AnimationSlideDirection.Random:
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0: myUpdatedPosition.y -= Screen.height; break;
                        case 1: myUpdatedPosition.y += Screen.height; break;
                        case 2: myUpdatedPosition.x += Screen.width; break;
                        case 3: myUpdatedPosition.x -= Screen.width; break;
                    }
                } break;
            case AnimationSlideDirection.Auto:
                {
                    if (position.x > (Screen.width / 2))
                    {
                        if (position.y > (Screen.height / 2))// Top Right
                        {
                            if (Screen.width - position.x < Screen.height - position.y)
                                myUpdatedPosition.x += Screen.width;
                            else
                                myUpdatedPosition.y += Screen.height;

                            break;
                        }
                        else // Down Right
                        {
                            if (Screen.width - position.x < position.y)
                                myUpdatedPosition.x += Screen.width;
                            else
                                myUpdatedPosition.y -= Screen.height;

                            break;
                        }
                    }
                    else
                    {

                        if (position.y > (Screen.height / 2)) // Top Left
                        {
                            if (position.x < Screen.height - position.y)
                                myUpdatedPosition.x -= Screen.width;
                            else
                                myUpdatedPosition.y += Screen.height;

                            break;
                        }
                        else // Down Left
                        {
                            if (position.x < position.y)
                                myUpdatedPosition.x -= Screen.width;
                            else
                                myUpdatedPosition.y -= Screen.height;

                            break;
                        }
                    }
                }
        }

        return myUpdatedPosition;
    }

 

    // Update is called once per frame
    void Update()
    {

    }

}
