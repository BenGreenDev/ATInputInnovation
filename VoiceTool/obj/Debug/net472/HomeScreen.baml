   M S B A M L   `   `   `  ����D  @VoiceTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:VoiceTool	VoiceTool  5   	       VoiceTool.HomeScreen   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:VoiceTool     5   	     � Title$  
HomeScreen��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� $
��Left=�6   $��	10,10,0,0q�6/   $'��!Welcome To The Voice Control Tool��6B   $
��Wrap=�6k   $	��Top=�6   $��24(�6�   �� 5   
   $
��Left=�6   $��	10,70,0,0q�6/   $%��Step 1 - Load A Control Scheme:��6B   $
��Wrap=�6i   $	��Top=�6}   �� 5   
   -   5        $��Load Control Scheme��5   +   $
��Left=�6I   $��
197,68,0,0q�6d   $	��Top=�6x   �� 5   
   $
��Left=�6   $��
10,124,0,0q�6/   $.��(Step 2 - Select Twitch or Voice Control:��6C   $
��Wrap=�6s   $	��Top=�6�   $	��216��6�   �� 5   
   -   5        $��Twitch Control��5   $   $
��Left=�6=   $��226,122,0,0q�6X   $	��Top=�6m   $��88��6�   �� 5   
   -   5        $��Voice Control��5   #   $
��Left=�6;   $��338,122,0,0q�6V   $	��Top=�6k   $��79��6�   �� 5   
   $
��Left=�6   $��
10,176,0,0q�6/   $I��CStep 3 - Target the Desired Window in the Control Screen, and Play!��6C   $
��Wrap=�6�   $	��Top=�6�   �� 5   
   $
��Left=�6   $��
10,219,0,0q�6/   $
��Wrap=�6C   $����As far as I'm aware, you can edit your control scheme even when in a twitch, or voice control screen by reopening step 1, and pressing confirm. However, since this is a prototype there may be issues, for I cannot test all possible outcomes whilst keeping everything thread safe.��6W   $	��Top=�6u  $	��749��6�  �� 5   
   $
��Left=�6   $��
10,277,0,0q�6/   $
��Wrap=�6C   $����Whilst I've tested as much as I can, due to the low level nature of this application some issues may arise since dozens of threads and input requests are being processed. Inputs are set to only be sent when you're targeting the window you said you wanted to send inputs to. Therefore, in the rare event of unexpected behaviours, tab out of that screen.��6W   $	��Top=�6�  5      5      