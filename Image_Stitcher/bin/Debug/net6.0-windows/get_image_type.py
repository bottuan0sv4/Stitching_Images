import os
import cv2
import argparse
# import time
# import sys

# Parse argument from command prompt to get path
ap = argparse.ArgumentParser(description = "Insert path contains images.")
ap.add_argument("path", help = "Image path must be in brackets", type = str)
arg = vars(ap.parse_args())

# Check 1st image
filepath = arg['path'] + "\\" + (os.listdir(arg['path'])[0])

# Read and check image type
img = cv2.imread(filepath)
print(img.shape[2])
# sys.stdout.flush()
# time.sleep(1)
