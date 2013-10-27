Edge-Ditector
=============

C# edge detector with sobel,prewitt,robert, and canny edge detection algorithms 


Edge Detection Algorithms used in the application
Sobel operator
	The image is loaded as a bitmap and converted to grayscale image with 32 bpp.
	Do convolution with following masks for that gray scale image and calculate X and Y components of color gradient
Masks,   Dx=   ■(-1&0&1@-2&0&2@-1&0&1)       Dy=   ■(1&2&1@0&0&0@-1&-2&-1)
	Get the sum of square of the corresponding X and Y gradients and take square root of it
G = √(G_x^2+G_y^2 )
	The resultant matrix is converted to a bitmap.
Prewitt operator
	All the steps are same as above except the convolution masks. Those are as follows.
Masks,   Dx=   ■(-1&0&1@-1&0&1@-1&0&1)       Dy=   ■(1&1&1@0&0&0@-1&-1&-1)
Robert operator
	All the steps are same as above except the convolution masks. Those are as follows.
Masks,   D-=   ■(1&0@0&-1)       D+=   ■(0&1@-1&0)
	The gradients are taken in perpendicular angles each have 45 degrees angles with horizontal direction.
Canny Edge Detector
	This algorithms follows several steps
	The image is loaded as a bitmap and converted to grayscale image with 32 bpp.
	It is smoothed using the Gaussian kernel. The size and the sigma values can be specified
	Find gradient magnitude and directions using Sobel operator.
Gradient Magnitude |G| = √(G_x^2+G_y^2 )
Gradient Direction Θ = arctan⁡(G_x/G_y )
	The magnitude of gradients are approximated to nearest angles of 0, 45, 90, 135 degrees. Select the local maxima of gradient magnitudes using the approximated directions. (no-maxima suppression)
E:g; if the angle is 0 degrees, the pixel is considered as a potential edge pixel, if its magnitude is greater than that of the west and east pixels.
	Then the minimum and maximum thresholds are applied. The pixels which are below the minimum threshold are ignored whereas the pixels that are above the maximum threshold is considered as strong pixels. Pixels in between the two thresholds are taken as week edges. 
	Traverse through the image connecting pixels find in non-maxima suppression and strong and weak edges find in double thresholding in order to get a complete set of edges
