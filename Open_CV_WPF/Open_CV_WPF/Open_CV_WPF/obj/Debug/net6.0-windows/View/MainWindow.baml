   M S B A M L   `   `   `  ����F  BOpen_CV_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Open_CV_WPF.ViewOpen_CV_WPF.View  5   	   !    Open_CV_WPF.View.MainWindow   5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Open_CV_WPF.View     5   	     � Title$  
MainWindow��5	   	   $	��680��5
   	   $
��1000��5   	   $��14(�5   	   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��70 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $	��150 �6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��1��6,   .+�5      �� $��Slider Value:��6   $��Center=�6-   $��8q�6H   �� 5      $��1��6    � Minimum$ 0Q�6%    � Maximum$	 255Q�61    � Value$	 120Q�6?   $��8q�6K      MaterialDesignSlider#��[  5      �� 5      $��Slider Value:��6   $��Center=�6-   $��8q�6H   �� 5      $��1��6   $ 0Q�6%   $	 255Q�61   $	 120Q�6?   $��8q�6K   #��[  5      �� 5      $��Slider Value:��6   $��Center=�6-   $��8q�6H   �� 5      $��1��6   $ 0Q�6%   $	 255Q�61   $	 120Q�6?   $��8q�6K   #��[  5      5       �� 5"   
   $��Uniform=�6/   ��6   �� *VideoSource+6/   �� 5#   
   $��1��6   $ 
Horizontal=�6"   .+�5$      �� $��Right=�6   $	��150��61   $��Add new Note��6>   $��5,5,5,5q�6U   ���	#FF26FF6F��5%      �� *ShowWindowAddNoteCommand+ � 	Resources 5&       �� 
TargetType	 �� Property
 �� Value��%^   '��       �� 5'      " ��6   .�5(      �� 	 �@��CornerRadius$
 5��65   5)      5*      5+      �� 5-   
   $��25��6   $
��Left=�5.      $	��150��5/      $��10,5,3,0q�50      $��1��52      ��51      �� *	AllCams+ �� SelectedItem 52       �� *SelectedCam+�� 53   
   $
��Left=�6   $��10q�6,   $	��Top=�68   $��60��6P   $��50��6[   .��54      �� .+�55      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ PlayCircleOutline=�6*   $��5q�6C   �� 56      $
��Play��6   $��Center=�6(   $��Center=�6E   57      58      �� 5;   
   $
��Left=�6   $��10q�6,   $	��Top=�68   $��80��6P   $��50��6[   .��5<      �� .+�5=        $ FiberManualRecord=�6*   $��5q�6C   �� 5>      $��Record��6   $��Center=�6*   $��Center=�6G   5?      5@      �� 5C   
   $
��Left=�6   $��10q�6,   $	��Top=�68   $��70��6P   $��50��6[   .��5D      �� .+�5E        $ PauseCircleOutline=�6*   $��5q�6D   �� 5F      $��Pause��6   $��Center=�6)   $��Center=�6F   5G      5H      �� 5K   
   $
��Left=�6   $��10q�6,   $	��Top=�68   $��70��6P   $��50��6[   .��5L      �� .+�5M        $ StopCircleOutline=�6*   $��5q�6C   �� 5N      $
��Stop��6   $��Center=�6(   $��Center=�6E   5O      5P      �� 5S   
   $
��Left=�6   $��10q�6,   $	��Top=�68   $��90��6P   $��50��6[   ��5T      �� *ButtonOpenFileCommand+.��5V      �� .+�5W        $ FolderOpenOutline=�6*   $��5q�6C   �� 5X      $��	Open File��6   $��Center=�6-   $��Center=�6J   5Y      5Z      5[      5\      5]      