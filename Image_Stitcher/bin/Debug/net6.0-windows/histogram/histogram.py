import os
import argparse
import skimage.io
import numpy as np
import skimage.color
from skimage import img_as_ubyte
from matplotlib import pyplot as plt

# Parse argument from command prompt to get path
ap = argparse.ArgumentParser(description = "Insert path contains images.")
ap.add_argument("path", help = "Images path must be in brackets", type = str)
arg = vars(ap.parse_args())

# Read in the image and turn to grayscale
image = skimage.io.imread(fname = arg['path'], as_gray = True)
image = img_as_ubyte(image)

# Plot histogram of grayscale image
histogram, bin_edges = np.histogram(image, bins=256, range=(0, 256))
histogram = histogram.flatten().tolist()

# Print histogram to terminal
print(histogram)
