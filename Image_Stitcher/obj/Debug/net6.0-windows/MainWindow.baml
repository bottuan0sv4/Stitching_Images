   M S B A M L   `   `   `  ����I  EImage_Stitcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Image_StitcherImage_Stitcher  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   'clr-namespace:PanAndZoom
PanAndZoom  5   	       Image_Stitcher.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Image_Stitcher     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   (PAZclr-namespace:PanAndZoom     5   	   ] YXceed.Wpf.Toolkit.NET5, Version=4.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5	   	   -   5          �Name$  Window��5   	    � Title$ Image Stitcher��5   	   $	��768��6    $
��1366��6-   $
��1366��6:   $	��768��6J    � WindowStyle$ SingleBorderWindow=�6Z    � WindowStartupLocation$ CenterScreen=�6{    � Icon$ icon.ico��6�   ��5      �� 5   
   .��5      (� $��0,-17,0,-30q�6   .��5   
   � -   5         �Name$ 	main_grid��5      $
��1366��6"   $	��768��62   $
��1423��6B   ���	#FFCCCCCC$	��797��6f    � ColumnDefinitions 5      �� 5      $	��42* �6#   �� 5      $��1381* �6#    � RowDefinitions 5      �� 5      $
��Auto �6    .+�5      �� -   5         ��Name$ txt_path��5      $
��Left=�6(   $��	11,17,0,0q�6C   	 �� TextWrapping$
	 Wrap=�6V   $	��Top=�6j   $	��232��6�   $��44��6�   
 �� 
IsReadOnly$

 True��6�   $��1��6�   �� 5      $
��Path��6   $
��Left=�6#   $��8,17,0,0q�6>   $	��Top=�6P   $��Input)�6h   $��39��6{   $
��Bold"�6�   $��2��6�   �� 5      -   5        $ txt_info��5      $
��Left=�6(   $��	11,76,0,0q�6C   $
	 Wrap=�6V   $	��Top=�6j   $	��280��6�   $��44��6�   $

 True��6�   $��1��6�   �� 5      $
��Info��6   $
��Left=�6#   $��8,76,0,0q�6>   $	��Top=�6P   $��Input)�6h   $
��Bold"�6|   �� 5      -   5         ��Name$ 
list_image��5      $	��Top=�6+   $
��Left=�6C   $��
11,135,0,0q�6^   $	��280��6r   $	��628��6�   $	��628��6�   $	��270��6�   $	��284��6�   $	��625��6�   $��1��6�    �� View 5      �� 5       .P�5!      �� $��ID��6)    �� Width$ 30��67    �� DisplayMemberBinding 6D   �� *ID+�� 5"      $��Images��6)   $	 250��6;    �� CellTemplate 5#      �� 5$   "   .R�5%   &   �� $	��180��6I   $	��180��6U   ��6,   ��  �� Path$
 Path�6e   5&   #   5(      5+      5-      �� 5/      -   5         ��Name$ btn_load��5/      $
��Load��6'   $
��Left=�66   $��
248,17,0,0q�6Q   $	��Top=�6e   $��43��6}   $��44��6�   $
��Lato)�6�   $��1��6�    � 	Resources 50       �� 
TargetType �� Property �� Value��%^   '��       �� 51      " ��6   .�52      ��  �@��CornerRadius$ 5��69   53      54      55      �� 56      $
��Left=�6   $	��103��63   $��
308,17,0,0q�6@   
�������$	��Top=�6b   $
��1043��6z   ���	#FFBABABA$��1��6�   �� 57      $��Output Filename��6   $
��Left=�6.   $��
314,23,0,0q�6I   $	��Top=�6]   $��Input)�6u   $
��Bold"�6�   $��13                (�6�   $��1��58      �� 59      -   5        $ btn_execute��59      $��Execute��6*   $
��Left=�6<   $��1023,64,0,0q�6W   $	��Top=�6l   $��36��6�   $	��104��6�   $��19(�6�   $
��Bold"�6�   $
��Lato)�6�    J� RenderTransformOrigin$ 0.494,0.048&�6�   $��1��6   5:      ��%^   '��       �� 5;      " ��6   .�5<      ��  �@��CornerRadius$ 5��69   5=      5>      5?      �� 5@      $��308,598,0,0q�6   $
��Left=�6-   $	��165��6H   $
��1045��6U   
�������$	��Top=�6p   ���	#FFBABABA$��1��6�   �� 5A      $��Wholeboard Histogram��6   $
��Left=�63   $��314,598,0,0q�6N   $	��Top=�6c   $��Input)�6{   $
��Bold"�6�   $��17(�6�   $	��226��6�   $��1��6�   �� 5B      -   5        $ 	btn_small��5B      $	��>-<��6(   $
��Bold"�6<   $
��Left=�6N   $��446,642,0,0q�6i   $	��Top=�6~   $��26��6�   $��40��6�   $ 0.101,-0.679&�6�   $��1��6�    5C      ��%^   '��       �� 5D      " ��6   .�5E      ��  �@��CornerRadius$ 5��69   5F      5G      5H      �� 5I      -	   5        $ btn_big��5I      $	��<->��6&   $
��Bold"�6:   $
��Left=�6L   $��504,642,0,0q�6g   $	��Top=�6|   $��26��6�   $��40��6�   $��1��6�    5J      ��%^   '��       �� 5K      " ��6   .�5L      ��  �@��CornerRadius$ 5��69   5M      5N      5O      %  Xceed.Wpf.Toolkit.IntegerUpDown  5Q      -
   5        $��
select_min��5Q   \   $��25��6!   $��70��6-   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]]   MouseWheelActiveTrigger$ 	MouseOver=�68      	AllowSpin$
 True��6n   ,  &Xceed.Wpf.Toolkit.Primitives.InputBase   AllowTextInput$ False��6   $��392,685,919,68q�6�      Value 06�   �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Int32, System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]]   	Increment 106�      Maximum 2546�      Minimum 06�   $��1��6�   �� 5R      $	��Min��6   $��14(�6"   $��353,684,0,0q�60   $
��Left=�6E   $	��Top=�6`   $��Input)�6x   $
��Bold"�6�   $��1��6�     5S      -   5        $��
select_max��5S   \   $��25��6!   $��70��6-   $ 	MouseOver=�68   $
 True��6n   $ False��6   $��533,685,778,68q�6�    2556�    106�    2556�    06�   $��1��6  �� 5T      $	��Max��6   $��14(�6"   $��494,684,0,0q�60   $
��Left=�6E   $	��Top=�6`   $��Input)�6x   $
��Bold"�6�   $��1��6�      PanAndZoom.ZoomBorder  5U      -   5          Name$ border��5U      $��308,135,28,185q�6-   $
}�True��6E   
�������$��1��6k      Child. 5V      �� -   5        $��
main_image��5V      $	��458��6*   $��0,0,0,0q�67   $	��996��6H     �� BitmapScalingMode$  NearestNeighbor=�6T   ! J� BitmapEffect! 5W      �� 5X      " �� 
KernelType$" Gaussian=�6+   $  HighQuality=�6A   # �� EdgeMode$# Aliased=�6o   $ �� Radius$ 6�   �� *image_quality+5Z      5[      �� 5\      $��Board Scale (1~100)��6   $
��Left=�62   $��
314,55,0,0q�6M   $	��Top=�6a   $��Input)�6y   $
��Bold"�6�   $��13(�6�   $ 0.244,2.086&�6�   $��1��6�   �� 5]      $<��6* If there is no barcode, it is replaced by Date Time.��6   $
��Left=�6U   $��
883,26,0,0q�6p   $	��Top=�6�   $	��385��6�   $��16(�6�   $��Italic$�6�   $��1��6�   #  LiveCharts.Wpf.CartesianChart  5^      &   LiveCharts.Wpf.Charts.Base.Chart%   LegendLocation$
% None=�6E   &   Zoom$& Xy=�6[   
'   Pan$' Xy=�6e   $��685,598,28,8q�6n   $��1��6�   (   Series( 6!   �� *SeriesCollection+)   AxisX) 5`        LiveCharts.Wpf.Axis  *   Title$* Grayscale Values��6   +   
Foreground
+ ��+��,   FontSize$, 13Q�6j   -   Labels- 68   �� *Labels+.   MinValue. 6x   �� *min_h+/   MaxValue/ 6�   �� *max_h+6�   0   AxisY0 5c        $* Pixel Count��6   
+ ��+��$, 13Q�6p   $. 0Q�6~   1   LabelFormatter1 63   �� *	Formatter+/ 6�   �� *MaxAxisValue+6�   5e        5f      -   5        $��select_zoom��5f   ]   $��25��6!   $��70��6-   $ 	MouseOver=�68   $
 True��6p   $ False��6�   $��474,60,837,694q�6�    106�    1006�    106�    106�   $��1��6  �� 5g      -   5        $��combobox_name��5g      $
��Left=�6,   $��
446,23,0,0q�6G   $	��Top=�6[   $	��209��6s   2 �� SelectedIndex$2 0��6   $��26��6�   3 �� 
IsEditable$
3 True��6�   $��1��6�   5i      �� 5j      $��Image Quality (60~100)��6   $
��Left=�65   $��
314,87,0,0q�6P   $	��Top=�6d   $��Input)�6|   $
��Bold"�6�   $��13(�6�   $ 0.244,2.086&�6�   $��1��6�     5k      -   5        4  Name$4 select_quality��5k   ]   $��25��6!   $��70��6-   $ 	MouseOver=�68   $
 True��6u   $ False��6�   $��498,92,813,662q�6�    1006�    1006�    606�    106�   $��1��6  5l      5m      5n      