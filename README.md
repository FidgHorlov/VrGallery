# Description
VR Gallery - an interactive journey through captivating artwork, with access to detailed information, all seamlessly integrated with Meta Quest 2 and could be adaptable to diverse client needs.
<br>
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/bd75f8f6-5c75-48d8-a22c-d114d07438a4" alt="drawing" width="500"/>
<br>
# Required:
* Meta Quest 2/3/Pro;
* Pictures format: ***.jpeg**, ***.jpg**;
* JSON file with picture information.
<br><br>
# How to use
1. Open SideQuest;
2. Press "Manage files on the Headset"
<br>
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/4770f7ec-1e84-424f-997b-92fdc00bc5ff" alt="drawing" width="800"/>

4. Go to the following path:
> sdcard/Android/data/com.FidgetLand.VrGallery/files
5. Add JSON file **ApplicationSettings**
* You can download the template from here -
* You can read how to edit Settings here.
6. Create a folder named -> **Images**.
7. Add all needed pictures there.

# JSON explanation
```
{
 "ImageModel":[
        {
            "Name":"The name of picture","Description":"Picture description","ImageName":"ImageName.jpg"
        }]
}
```
* The name of the picture - the short name of your picture.
* Picture description - a short description of your image.
* ImageName.jpg - the original image name with the format.

For example:
<br>
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/1b6080d2-928c-4f59-907b-4ade2aacf3b5" alt="drawing" width="500"/>
* The name of the picture is - > Buddha
* The description is - > The statue of Buddha.\r\nMadeira Island. Portugal
------
Made by Andrii Horlov.
