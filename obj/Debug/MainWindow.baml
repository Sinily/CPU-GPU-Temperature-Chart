   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	   	 chart   YourWPFProject.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   $	��400��5   &   $	��600��63     � Title  6	   �� *
GraphTitle+ � 	Resources 5	         	TextBrush �� Color��%)   &	        �� 5
   
   $ Black��6,   5      .��5      � .+�5   
   #   LiveCharts.Wpf.CartesianChart  -   5        $��chart��5      &    LiveCharts.Wpf.Charts.Base.Chart   Series 6*   �� *SeriesCollection+   AxisX 5         LiveCharts.Wpf.Axis     Title$ Time (Last Minute Only)��6      LabelFormatter 6;   �� *TimeFormatter+   	Separator 5         LiveCharts.Wpf.Separator  5         Step$ 1Q�6(   5      	   AxisY	 5        $ Temperature (°C)��6   
   MinValue$
 0Q�64      MaxValue$	 120Q�6A    5        5      $ 5Q�6(   5      5      � 5   
   -   5        $��
legendGrid��5      $
��Left=�6!   $	��Top=�6<   $��	50,10,0,0q�6T   .+�5!      �� ���	#80D3D3D3 �� CornerRadius$ 5��6,   ._�5"      ��  �� Orientation$ Vertical=�6   .+�5#      �� $ 
Horizontal=�6!   $��5q�6:   .+�5$      A� $��10��6"   $��10��6-   
������69   �� 5%      $	��CPU��6$   $��Center=�6/   #��[  6J   $	��5,0q�6r   $
��Bold"�6    � ContextMenu 5&      �� 5'   "   .@�5(   &   m� -   5        $��Show CPU Min/Max °C��5(   /   6L   5)   #   5+      5,      �� 5-      $ 
Horizontal=�6!   $��5q�6:   .+�5.      A� $��10��6"   $��10��6-   
��� E��69   �� 5/      $	��GPU��6$   $��Center=�6/   #��[  6J   $	��5,0q�6r   $
��Bold"�6    50      �� 51   "   .@�52   &   m� -   5        $��Show GPU Min/Max °C��52   /   6L   53   #   55      56      57      58      59      � 5:   
   -   5        $��cpuTempGrid��5:      $
��Left=�6"   $	��Top=�6=   $��
150,10,0,0q�6U   $y�	Collapsed=�6i   $��35��6�   .+�5>      �� ���	#801E90FF$ 5��6,   ._�5?      �  5@      �� 5A      .@�5B      m� -   5        $��Hide CPU Temperatures��5B   '   6F   5C      .+�5E      �� .+�5F      �� $
��Min:��6$   #��[  60   $
��Bold"�6X    �� TextAlignment$ Center=�6j   �� 5G      #��[  6@   $ Center=�6h   ��6$   �� *
cpuMinTemp+6h   �� 5H      $
��Max:��6$   #��[  60   $
��Bold"�6X   $ Center=�6j   �� 5I      #��[  6@   $ Center=�6h   ��6$   �� *
cpuMaxTemp+6h   5J      5K      5L      5M      � 5N   
   -   5        $��gpuTempGrid��5N      $
��Left=�6"   $	��Top=�6=   $��
250,10,0,0q�6U   $y�	Collapsed=�6i   $��35��6�   .+�5R      �� ���	#80FF4500$ 5��6,   ._�5S      �  5T      �� 5U      .@�5V      m� -   5        $��Hide GPU Temperatures��5V   '   6F   5W      .+�5Y      �� .+�5Z      �� $
��Min:��6$   #��[  60   $
��Bold"�6X   $ Center=�6j   �� 5[      #��[  6@   $ Center=�6h   ��6$   �� *
gpuMinTemp+6h   �� 5\      $
��Max:��6$   #��[  60   $
��Bold"�6X   $ Center=�6j   �� 5]      #��[  6@   $ Center=�6h   ��6$   �� *
gpuMaxTemp+6h   5^      5_      5`      5a      � 5b   
   -	   5        $��warningCpuGrid��5b      $��Center=�6%   $��Center=�6B   $y�Hidden=�6]   $��100,26,100.6,240q�6q   .+�5c      �� 
���   �$	��300��6(   $��90��64   $ 40��6@   
������$��5q�6k   ._�5d      � .+�5e      �� $��Warning!��6    $��30(�60   
�������$��	ExtraBold"�6Q   $��Center=�6h   $	��Top=�6�   $��70,5,70.5,0q�6�    J� RenderTransformOrigin$ 
0.54,0.035&�6�   $	��150��6�   �� 5f      $��CPU temperature too high!��6    $��20(�6A   
�������$��Center=�6b   $��Center=�6   $��26,46,25.6,9.6q�6�   $��34��6�   5g      5h      5i      � 5j   
   -
   5        $��warningGpuGrid��5j      $��Center=�6%   $��Center=�6B   $y�Hidden=�6]   $��100,26,100.6,40q�6q   .+�5k      �� 
���   �$	��300��6(   $��90��64   $ 40��6@   
��� E��$��5q�6j   ._�5l      � .+�5m      �� $��Warning!��6    $��30(�60   
�������$��	ExtraBold"�6Q   $��Center=�6h   $	��Top=�6�   $��70,5,70.5,0q�6�   $ 
0.54,0.035&�6�   $	��150��6�   �� 5n      $��GPU temperature too high!��6    $��20(�6A   
�������$��Center=�6b   $��Center=�6   $��26,46,25.6,9.6q�6�   $��34��6�   5o      5p      5q      �� 5r   
   -   5        $��About��5r      $��Right=�6!   $��Bottom=�6=   $��12(�6X   6f   � 5s   
   -   5        $��aboutBox��5s      $��Right=�6   $��Bottom=�6;   $y�Hidden=�6V   .+�5t      �� 
�������$	��200��6(   $	��170��64   
���   �$��1q�6U   ._�5u      �� .+�5v      �� $��
CP Project��6    $��18(�62   $
��Bold"�6@   $��Center=�6R   $��	0,10,0,10q�6o   �� 5w      $!��Creator: Viktoria Stefanova��6    $��Center=�6C   $��0,0,0,5q�6`   �� 5x      $��№: 379220��6    $��Center=�61   $��0,0,0,5q�6N   �� 5y      $��CST, FEA��6    $��Center=�60   $��0,0,0,5q�6M   �� 5z      $��TU-Sofia, Plovdiv branch��6    $��Center=�6@   $��0,0,0,10q�6]   �� 5{      -   5        $��Close��5{      $��Center=�6-   6J   5|      5}      5~      5      5�      