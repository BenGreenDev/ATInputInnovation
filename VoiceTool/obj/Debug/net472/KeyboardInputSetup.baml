   M S B A M L   `   `   `  ����D  @VoiceTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:VoiceTool	VoiceTool  5   	   "    VoiceTool.KeyboardInputSetup   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:VoiceTool     5   	     � Title$  
InputSetup��5   	   $	��350��6   $	��500��6)    � 
ResizeMode$ NoResize=�65   .��5	      �  � RowDefinitions 5
   
   �� 5      $��50 �6   6*   �� 5      $��* �6   6)   .+�5   
   �� -   5        $��ScreenTitle��5      $��0��6   $��	InputName��61   $
��Left=�6E   $��103,12,0,10q�6`   $	��150��6u   $��28��6�   �� 5   
    �� SelectionMode$ Single=�6   $��	10,32,0,0q�6)   $��Disabled=�6<   $��Disabled=�6r   
������ $��0q�6�   $
��Left=�6�   $	��380��6�   $	��141��6�   $	��Top=�6   J� RenderTransformOrigin$ -1.141,2.222&�6  $��1��6D  .@�5      �� .��5      	� -   5        $��PressAndRelease��5   �    	� 	GroupName$ Group1��6   $��Press and Release Key��61   $��0,0,0,10q�6Q   $��16��6c   $
��Left=�6o   $	��Top=�6�   $��12(�6�   5      �� 5      .��5      	� -   5        $��HoldKey��5   ~   $ Group1��6   $��Hold Key��61   $��0,0,0,10q�6D   $��16��6V   $
��Left=�6b   $	��Top=�6�   $��12(�6�   5      �� 5      .��5      	� -   5        $��
ReleaseKey��5   �   $ Group1��6   $��Release Key��61   $��0,0,0,10q�6G   $��16��6Y   $
��Left=�6e   $	��Top=�6�   $��12(�6�   5      �� 5      .��5      	� -   5        $��	ToggleKey��5   �   $ Group1��6   $��
Toggle Key��61   $��0,0,0,10q�6F   $��16��6X   $
��Left=�6d   $	��Top=�6�   $��12(�6�   5      5      �� 5!   
   $
��Left=�6   $	��173��6/   $��1��6<   
���   �$	��Top=�6X   $	��466��6p   $��10,0,0,0q�6|   �� 5"   
   -   5        $��NumberTextBox��5"       �� 	MaxLength$ 4��6R   $
��Left=�6`   $��
348,33,0,0q�6{   $��1��6�   $	��100��6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��120��6�   �� 5#   
   $
��Left=�6   $��1247,195,0,0q�6-   $��1��6C   $��TextBox��6P   $
 Wrap=�6_   $	��Top=�6s   $	��120��6�   �� 5$   
   $��Hold for (ms):��6   $
��Left=�6)   $��
246,29,0,0q�6D   $��1��6X   $	��Top=�6e   �� 5%   
   $��
Input Type��6   $
��Left=�6%   $��19,5,0,0q�6@   $��1��6R   $	��Top=�6_   �� 5(   
   -   5        $��
KeyTextBox��5(       �� 
IsReadOnly$
 True��6�   	 �� IsReadOnlyCaretVisible$	 False��6�   
 �� IsUndoEnabled$
 False��6�   $
��Left=�6�   $��112,199,0,0q�6  $��1��6"  $
 Wrap=�6/  $	��Top=�6C  $	��120��6[  ��6$   �� *HotKey+ �� ElementName$ UserControl�� �� Mode$ OneWay=� �� TargetNullValue$ < not set >��6[  �� 5)   
   -   5        $��1��5)      $��Discard��6   $
��Left=�6F   $��315,224,0,0q�6a   $	��Top=�6v   $ 0.754,0.198&�6�   $��75��6�   $��28��6�   �� 5*   
   -	   5        $��1��5*      $��Submit��6   $
��Left=�6E   $��396,224,0,0q�6`   $	��Top=�6u   $��80��6�   $��28��6�   �� 5+   
   $
��Left=�6   $��
10,195,0,0q�6+   $��1��6?   $��Enter Key/Keys:��6L   $	��Top=�6f   $��97��6~   �� 5,   
   $
��Left=�6   $��10,0,0,0q�6/   $��Activation Word:��6A   $
��Wrap=�6Y   $��Center=�6m   5-      5.      