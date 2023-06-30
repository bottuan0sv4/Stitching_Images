# Image Stitcher (Currently in Development)
This is a personal project.  
The purpose of this application is to stitch multiple images vertically or horizontally.

## Table of Contents
* [General Info](#general-info)
* [Technologies](#technologies)
* [Algorithm Implementation](#algorithm-implementation)
* [Resources](#resources)
* [References](#references)
* [Documents](#documents)

## General Info
The UI for this application is created using C# with WPF Framework.</br>
The algorithm behind it is implemented using Python.

## Technologies
### User Interface
* Microsoft Visual Studio 2022 Community Edition (ver 17.3.3) using WPF Framwork with .NET 6.0 desktop development.
* LiveCharts.Wpf (Nuget Package) by Beto Rodriguez.
* Extended.Wpf.Toolkit (Nuget Package) by Xceed.
### Algorithm
* Python 3.x (preferred 3.9.6)
* OpenCV Library 4.x (preferred 4.5.5)
* Numpy Library 1.x (preferred 1.19.5)
* Imutils Module 0.x (preferred 0.5.4)
* Matplotlib Library 3.x (preferred 3.4.2)
* Skimage Library 0.x (preferred 0.19.3)

## Algorithm Implementation
For keypoints and features detection, the **Scale-Invariant Feature Transform (SIFT)** [1] algorithm is used from the **opencv** package.
</br>

<p align="justify">
Once the <b>keypoints</b> and <b>features descriptors</b> are obtained from a pair of images, <i>brute-force-matching</i> is performed using <b>Euclidean distance</b> as the metric. For each point in one image, two points with <i>lowest</i> Euclidean distance in the other image is obtained using <b>KNN algorithm</b> (indicating the top two matches). The reason we want the top two matches rather than just the top one match is because we need to apply David Lowe’s ratio test for false-positive match pruning.
</br>
</br>
With a list of matched points between two images, the <b>Affine Transformation Matrix</b> can be computed. However, since the images are taken by a camera that move accurately horizontally and vertically, we only need to extract the <b>Translation Matrix</b> from the transformation matrix.
</br>
</br>
Once a translation matrix is obtained, opencv's warp Affine function is used to transform the second image into the perspective of the first. The algorithm therefore is faster and more accurate compare to when we use homography matrix to warp image.
</br>
</br>
</p>

## Resources
<p align="justify">
[1] David Lowe, <b>"Distinctive Image Features from Scale-Invariant Keypoints"</b> - November, 2004 - International Journal of Computer Vision 60(2):91---110 - DOI: 10.1023/B:VISI.0000029664.99615.94 
</p>

## References
1. https://math.stackexchange.com/questions/237369/given-this-transformation-matrix-how-do-i-decompose-it-into-translation-rotati/417813#417813
2. https://stackoverflow.com/questions/54483794/what-is-inside-how-to-decompose-an-affine-warp-matrix

## Documents
1. [Software Requirement Specification](https://docs.google.com/document/d/15WrhWoT1YypFDTgjju1YcEVTrSuhIc3lM7sdHldyAN4/edit?usp=sharing)
2. [System Design Specification](https://docs.google.com/document/d/1pAWvcZ9oywys1ZpVrD5P6CMuZXhkgCsqDr2yt6ugUxk/edit?usp=sharing)
