   M S B A M L   `   `   `  ����D  @VoiceTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:VoiceTool	VoiceTool  5   	       VoiceTool.ChatControl   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:VoiceTool     5   	     � Title$  ChatControl��5   	   $	��800��6   $	��800��6*    � 
ResizeMode$ NoResize=�66   .��5	      � .+�5
   
   �� $	��300��6   $	��200��6   $
��Left=�6+   $	��Top=�6F   $��0q�6^   .@�5      �� .��5      �� $
��Left=�6   $2��,Please connect to a twitch chat server first��67   $
��Wrap=�6k   $��14(�6   $
��Bold"�6�   5      �� 5      .��5      �� $
��Left=�6   $��Bot Username��67   $
��Wrap=�6K   5      �� 5      .��5      �� -   5         ��Name$ Username��5      $
��Left=�6,   $��razznecrobot��6G    �� TextWrapping$
 Wrap=�6[   $	��120��6o   5      �� 5      .��5      �� $
��Left=�6   $��Bot OAuth Token��67   $
��Wrap=�6N   $	��Top=�6b   5      �� 5      .��5      �� -   5        $ OAuth��5      $
��Left=�6)   $*��$oauth:34yxbzlc5dqfpl80ev5szdo6zuzccp��6D   $
 Wrap=�6p   $	��120��6�   5      �� 5      .��5      �� $
��Left=�6   $"��Twitch Channel Name To Watch��67   $
��Wrap=�6[   $	��Top=�6o   5      �� 5      .��5      �� -   5        $ IRCChannelName��5      $
��Left=�62   $��letsplaystuff��6M   $
 Wrap=�6b   $	��120��6v   5      �� 5       .��5!      �� -   5         ��Name$ ConnectButton��5!      $��Connect��60   $
��Left=�6_   5"      5#      �� 5$   
   -   5         ��Name$ ChatLog��5$      $
��Left=�6&   $��433,253,0,0q�6A   $�� ��6V   $
��Wrap=�6^   $	��Top=�6r   $	��330��6�   $	��509��6�   ��5%      �� 5&       �� Color$ 	LightGray��6"   5(      �� 5*   
   $��371,1,9.667,675q�6   $��0q�6+   .@�5+      �� .��5,      �� $
��Wrap=�6   $?��9After connecting, please target the window and click play��60   $
��Bold"�6q   $��14(�6�   5-      �� 5.      .��5/      �� -   5        $��TargetWindow��5/      $	��208��60   $��Right=�6<   50      �� 51      .��52      �� -   5        $��Refresh��52      $��75��6C   53      54      �� 55   
   -   5        $��
PlayButton��55      $
��Play��60   $
��Left=�6?   $��710,120,0,0q�6Z   $	��Top=�6o   $��75��6�    J� RenderTransformOrigin$ -1.257,0.931&�6�   &   System.Windows.Controls.DataGrid  57   
   -	   5        $��ActivationGrid��57         
IsReadOnly$
 True��6)   	   AutoGenerateColumns$	 False��6;   $
��Left=�6W   $	��509��6r   $��
10,252,0,0q�6   $	��Top=�6�   $	��418��6�   
   Columns
 58      0  *System.Windows.Controls.DataGridTextColumn  59      ,  &System.Windows.Controls.DataGridColumn   Header$ Activation Word��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6>   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6J   ��  �� Path$ activationKeyword�  5:      $ 	Key Bound��6%   $	 185 68      
IsReadOnly$
 True��6E    6W   �� $ UIInputRepresentation�5<      �� 5=   
   $
��Left=�6   $��
10,231,0,0q�6/   $
��Wrap=�6C   $��Your Commands��6W   $	��Top=�6l   $��14(�6�   $
��Bold"�6�   �� 5>   
   $
��Left=�6   $��433,231,0,0q�6/   $
��Wrap=�6D   $0��*Detected Commands In Chat (Case Sensitive)��6X   $	��Top=�6�   $��14(�6�   $
��Bold"�6�   5?      5@      