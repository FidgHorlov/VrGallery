# Description
VR Gallery - an interactive journey through captivating artwork, with access to detailed information, all seamlessly integrated with Meta Quest 2 and could be adaptable to diverse client needs.
<br>
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/128df3d0-51a5-4c31-8e7b-b3134d5c9566" alt="Original image" width="800"/>
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
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/4770f7ec-1e84-424f-997b-92fdc00bc5ff" alt="Original image" width="800"/>

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
            "Name":"Rocks","Description":"Rocks VS Water. Madeira Island. Portugal","ImageName":"RocksMadeira.jpg"
        }]
}
```
* _Rocks_ - the short name of your picture.
* _Rocks VS Water. Madeira Island. Portugal_ - a short description of your image.
* _RocksMadeira.jpg_ - the original image name with the format.

### How it looks
<img src="https://github.com/FidgHorlov/VrGallery/assets/110767790/7c18895e-57b4-4147-b667-bc6e22d0dfa8" alt="Image description" width="700"/>
<br>

------
Made by Andrii Horlov.
