   M S B A M L   `   `   `  ���� F  BSaveMaestro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SaveMaestroSaveMaestro      SaveMaestro.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:SaveMaestro     A =Wpf.Ui, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  -     �Name$  SaveMaestro�� � Title$ SaveMaestro V1.2.2��$	��600��$	��800��$��20(�$
��Bold"�.���  J� RenderTransformOrigin$ 0.511,0.501&�.+��� $��4q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #462AD8�� � Offset$ 0Q�� $ #1A96A6��$
 0.75Q�� $ #8A16C1��$ 1Q����� $	 0,1&�$	 1,0&�.��� $ #060531��$ 0Q�� $ #1B1448��$
 0.75Q��� $��PS4 IP:��$
��Left=�$��34��$��7,12,0,0q�$	��Top=�$��77��$ 0.542,0.284&�
�������$��Microsoft YaHei UI Light)�$��Normal"�z�V� .~��� �� ��  �� Angle$ 0.404Q�T�  T� X$ 0.061Q�	 T� Y$	 0.023Q��� -   
 ��Name$
 	saveinput��$
��Save��$
��Left=�$��30��$��
10,239,0,0q�$	��Top=�$��61��$ 0.502,1.555&��� $��Socket Port:��$
��Left=�$��5,55,0,0q�$	��Top=�$ 0.5,0.5&�$	��127��$��20(�
�������$��Microsoft YaHei UI Light)�$��Normal"�z�V� .~��� �� �� $ -0.569Q�T� �� -    ��Name$ 
socketport��$
��Left=�$��
124,48,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��124��$��15(�$��45��$��Normal"�$ 	0.3,0.453&��� $��	FTP Port:��$
��Left=�$��	6,101,0,0q�$	��Top=�$��96��
�������$��Microsoft YaHei UI Light)�$��Normal"��� -   $ ftpport��$
��Left=�$��	94,94,0,0q�$
 Wrap=�$	��Top=�$��92��$��15(�$��43��$ 0.533,1.229&�$��Normal"��� -   $
 psip��$
��Left=�$��70,5,0,0q�$
 Wrap=�$	��Top=�$	��178��$��54��$��15(�$��Normal"�$ 1.242,1.097&��� $��PS4 Save Upload Path:��$
��Left=�$��	6,141,0,0q�$	��Top=�$	��216��
�������$��Microsoft YaHei UI Light)�$��Normal"��� -   $ psuploadpath��$
��Left=�$��222,135,0,0q�$
 Wrap=�$	��Top=�$	��216��$��40��$��15(�$��Normal"��� $��PS4 Mount Path:��$
��Left=�$��	6,180,0,0q�$	��Top=�$	��183��
�������$��Microsoft YaHei UI Light)�$��Normal"��� -   $ 	mountpath��$
��Left=�$��168,174,0,0q�$
 Wrap=�$	��Top=�$	��241��$��15(�$��Normal"�$��42���� $��Account ID Converter��$
��Left=�$��	552,5,0,0q�$	��Top=�$	��217��
�������$��Normal"�$��Microsoft YaHei UI Light)��� $��	Username:��$
��Left=�$��
552,47,0,0q�$	��Top=�$��15(�
�������$��Normal"�$��Microsoft YaHei UI Light)�{�.  �� -   $
 	convertid��$��Convert��$
��Left=�$��
635,90,0,0q�$	��Top=�$��87��$��32��$ 0.623,0.434&��� -	   $ ps_username��$
��Left=�$��
635,42,0,0q�$
 Wrap=�$	��Top=�$	��134��$��15(�$��Normal"�$��43���� $	��ID:��$
��Left=�$��552,140,0,0q�$	��Top=�$��48��
�������$ 0.021,0.548&�$��Normal"�$��Microsoft YaHei UI Light)��� -
   $ idblock��$
��Left=�$��588,136,0,0q�$
 Wrap=�$	��Top=�$	��181��$��15(�$��39���� -    ��Name$ username_block��$
��Left=�$��566,167,0,0q�$
��Wrap=�$	��Top=�
��������� -   $
 Resign_button��$��68,338,629,190q�$��Resign��$ 0.789,0.251&��� -   $
 Decrypt_button��$��240,338,457,190q�$��Decrypt��$ 0.789,0.251&��� -   $
 Import_button��$��418,338,279,190q�$��Import��$ 0.789,0.251&��� -   $
 Reregion_button��$��576,338,98,190q�$��	Re-region��$ 0.789,0.251&��� -   $
 customcrypto_button��$��240,435,0,0q�$$��Custom Encryptions/Decryptions��$ 0.789,0.251&�$	��Top=�$	��244��$��39��