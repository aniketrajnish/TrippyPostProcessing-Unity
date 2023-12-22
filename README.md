# TrippyPostProcessing-Unity
 A trippy post-processing and render feature for Unity's URP based on [Sobel Filter Edge Detection](https://en.wikipedia.org/wiki/Sobel_operator). <br> I wrote this so that the next time I get high, I would work on projects with this ON :] 

https://github.com/aniketrajnish/TrippyPostProcessing-Unity/assets/58925008/6e33b53f-f52b-4550-b358-e1837a76a267

## Usage
* Download the `.unitypackage` from the [Releases Section](https://github.com/aniketrajnish/TrippyPostProcessing-Unity/releases/tag/v001).
  * `Trippy_PP_min_v001.1.unitypackage` just contains the backend for the post-processing.
  * `Trippy_PP_all_v001.1.unitypackage` contains the backend as well as a demo scene with the implementation for reference.
* Open/Create a new URP Unity project.
* Import the package into the project.
* Enable the Trippy Renderer Feature in your URP Renderer (generally in `Assets/Settings` folder) by adding the `Trippy Renderer Feature` to the Renderer Features.
* Control the Renderer Feature's properties by adding a `Global Volume` component to your scene.
* Add `Makra/Trippy` override to the Volume component. The custom post-processing contains the following components- <br><br>
  | Name          | Description                                                                                       |
  |---------------|---------------------------------------------------------------------------------------------------|
  | Enable Effect | Toggles the effect on or off                                                       |
  | Intensity     | Intensity of the Trippy effect                                                       |
  | Edge Color    | Color of the edges                                                |
  | Edge Width    | Width of the edges                                    |
  | Base Color    | Base color of the environment                          |
  | Use Gradient  | Toggles if a gradient texture is used as the edge color                                       |
  | Gradient Tex  | The gradient texture to be used                   |

* If you wanna have some fun consider adding the `TrippyVolumeAnimator.cs` script to the Volume component ;)

https://github.com/aniketrajnish/TrippyPostProcessing-Unity/assets/58925008/c0dd9c55-ced7-45ee-85c3-e9a21e509cb7

## Contributing
Contributions to the project are welcome. Currently working on:
* A better algorithm for edge width calculation. 
  
## License
MIT License
