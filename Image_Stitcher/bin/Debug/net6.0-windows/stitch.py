# Import necessary library
import os
import cv2
import shutil
import imutils
import argparse
import numpy as np
from pathlib import Path
import matplotlib.pyplot as plt



# SET UP
MINIMUM_MATCH_POINTS = 20

## Parse argument from command prompt to get path
ap = argparse.ArgumentParser(description = "Insert path contains images and output file name.")
ap.add_argument("path", help = "Images path must be in brackets", type = str)
ap.add_argument("name", help = "Output file name must be in brackets", type = str)
arg = vars(ap.parse_args())

## Path to images
path = arg['path'] + "\\"

## Use dictionary to contain images
image_lib = {}
j = 1
for i in os.listdir(path):
    # Filter out images file only
    if i.endswith((".jpg", ".png", ".tiff", ".jpeg")):
        image_lib[j] = path + i
        j += 1
image_lib_len = len(image_lib)

## Create folder to contain stitches images
Path(path + "Fin").mkdir(parents = True, exist_ok = True)



# MAIN FUNCTION
def stitch_image(img_a, img_b, direction, path):
    # Read images (IMPORTANT!: IMAGES MUST BE IN CORRECT ORDER L -> R, D -> U)
    img_a = cv2.imread(img_a)
    img_b = cv2.imread(img_b)

    # Stitch images vertically (1) or horizontally (0)
    if direction == 1:
        img_b = cv2.resize(img_b, (img_a.shape[1], img_b.shape[0]))
        img_a = cv2.rotate(img_a, cv2.ROTATE_90_CLOCKWISE)
        img_b = cv2.rotate(img_b, cv2.ROTATE_90_CLOCKWISE)
        x = 50
    else:
        x = 100

    # Convert images to grayscale (for testing only)
    img_a_gray = cv2.cvtColor(img_a, cv2.COLOR_BGR2GRAY)
    img_b_gray = cv2.cvtColor(img_b, cv2.COLOR_BGR2GRAY)

    # Create a mask
    mask_1 = np.zeros(img_a.shape[:2], dtype=np.uint8)
    mask_2 = np.zeros(img_b.shape[:2], dtype=np.uint8)

    cv2.rectangle(mask_1, (img_a.shape[1] - x,0), (img_a.shape[1],img_a.shape[0]), (255), thickness = -1)
    cv2.rectangle(mask_2, (0,0), (x,img_b.shape[0]), (255), thickness = -1)

    # Initialize SIFT
    if direction == 0:
        sift = cv2.SIFT_create(nfeatures = 10000, nOctaveLayers = 10, contrastThreshold = 0.0001, edgeThreshold = 35, sigma = 3)
    else:
        sift = cv2.SIFT_create(nfeatures = 10000, nOctaveLayers = 10, contrastThreshold = 0.0001, edgeThreshold = 35, sigma = 1)

    # Compute Keypoints and Descriptors
    kp_a, desc_a = sift.detectAndCompute(img_a, mask_1)
    kp_b, desc_b = sift.detectAndCompute(img_b, mask_2)

    # Match Keypoints using Brute-force method, Euclidean Distance and KNN
    dis_matcher = cv2.BFMatcher(cv2.NORM_L2)
    matches_list = dis_matcher.knnMatch(desc_a, desc_b, k=2)

    # Filter out matches
    good_matches_list = []
    for m in matches_list:
        if m[0].distance < 0.75 * m[1].distance:
            good_matches_list.append(m)

    matches = np.asarray(good_matches_list)
    # Should leave at 4?
    if len(matches[:,0]) >= MINIMUM_MATCH_POINTS:
        src = np.float32([ kp_a[m.queryIdx].pt for m in matches[:,0] ]).reshape(-1,1,2)
        dst = np.float32([ kp_b[m.trainIdx].pt for m in matches[:,0] ]).reshape(-1,1,2)
    else:
        return 0

    # Calculate Affine Matrix
    H = cv2.estimateAffine2D(dst, src, False)
    H = np.float32(H[0])

    # Only get translation matrix
    H[0,0] = 1.0;
    H[0,1] = 0.0;
    H[1,0] = 0.0;
    H[1,1] = 1.0;

    # Warp image
    result = cv2.warpAffine(img_b, H, (img_a.shape[1] + img_b.shape[1], img_a.shape[0]))
    result[:, 0:img_a.shape[1], :] = img_a[:, :, :]

    # Cut out the dark part
    y_nonzero, x_nonzero, _ = np.nonzero(result)
    result = result[np.min(y_nonzero):np.max(y_nonzero), np.min(x_nonzero):np.max(x_nonzero)]

    # Rotate image back if stitch vertically
    if direction == 1:
        result = cv2.rotate(result, cv2.ROTATE_90_COUNTERCLOCKWISE)

    # Save image
    cv2.imwrite(path + "result.jpg", result)

    return 1



# STITCH PROCESS
## Horizontally
k = 1
l = 0
for i in image_lib:
    if k == 1:
        x = image_lib[i]
        k += 1
    a = stitch_image( x, image_lib[i+1], 0, path)
    if a == 1:
        x = path + "result.jpg"
    if i == (image_lib_len - 1):
        shutil.copy(path + "result.jpg", path + "Fin\\result_" + str(l) + ".jpg")
        break
    elif a == 0:
        shutil.copy(path + "result.jpg", path + "Fin\\result_" + str(l) + ".jpg")
        l += 1
        i += 1
        k = 1

## Clean up
os.remove(path + "result.jpg")
image_lib.clear()

## Count variable
j = 1
for i in os.listdir(path + "Fin"):
    image_lib[j] = path + "Fin\\" + i
    j += 1
image_lib_len = len(image_lib)

## Vertically
k = 1
l = 0
for i in image_lib:
    if k == 1:
        x = image_lib[i]
        k += 1
    a = stitch_image( x, image_lib[i+1], 1, path + "Fin\\")
    if a == 1:
        x = path + "Fin\\result.jpg"
    if i == (image_lib_len - 1):
        break

for i in image_lib.values():
    os.remove(i)

os.rename(path + "Fin\\result.jpg", path + "Fin\\" + arg['name'] + ".jpg")
