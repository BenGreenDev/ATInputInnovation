   M S B A M L   `   `   `  ����D  @VoiceTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:VoiceTool	VoiceTool  5   	       VoiceTool.SpeechControl   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:VoiceTool     5   	     � Title$  SpeechControl��5   	   $	��800��6   $	��800��6,   .��5	      � .+�5
   
   �� $��85��6   $	��Top=�6   $
��Left=�66   .@�5      �� .��5      �� $
��Wrap=�6   $-��'Please target the window and click play��60   5      �� 5      .��5      �� -   5        $��TargetWindow��5      $	��208��60   5      �� 5      .��5      �� -   5        $��Refresh��5      $��75��6C   5      5      �� 5   
   -   5        $��
PlayButton��5      $
��Play��60   $
��Left=�6?   $��
247,64,0,0q�6Z   $	��Top=�6n   $��75��6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��ActivationGrid��5         
IsReadOnly$
 True��6)      AutoGenerateColumns$ False��6;   $
��Left=�6W   $	��636��6r   $��
10,125,0,0q�6   $	��Top=�6�   $	��435��6�      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Activation Word��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6>   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6J   ��  �� Path$ activationKeyword�  5      $ 	Key Bound��6%   $	 225 68      
IsReadOnly$
 True��6E    6W   �� $ UIInputRepresentation�5      �� 5   
   $
��Left=�6   $��	10,97,0,0q�6/   $
��Wrap=�6B   $��Your Commands��6V   $	��Top=�6k   $��20(�6�   �� 5   
   $
��Left=�6   $��
459,97,0,0q�6/   $
��Wrap=�6C   $��Detected Commands��6W   $	��Top=�6p   $��20(�6�   �� 5   
   -   5        	 ��Name$	 ChatLog��5      $
��Left=�6%   $��459,125,0,0q�6@   $�� ��6U   $
��Wrap=�6]   $	��Top=�6q   $	��324��6�   $	��636��6�   ��5      �� 5       
 �� Color$
 	LightGray��6"   5"      �� 5#   
   $
��Left=�6   $��
346,21,0,0q�6/   $	��257��6C   $
��Wrap=�6O   $����If inputs aren't working when pressing play, make sure you're in game window, and last resort update the target on the left. Also check your microphone is the default input device.��6c   $	��Top=�6  5$      5%      