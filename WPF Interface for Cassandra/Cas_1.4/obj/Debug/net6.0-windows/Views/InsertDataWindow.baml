   M S B A M L   `   `   `  ����B  >Cas_1.4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Cas_1._4.ViewsCas_1._4.Views  5      9!clr-namespace:Cas_1._4.ViewModelsCas_1._4.ViewModels  5      %    Cas_1._4.Views.InsertDataWindow   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Cas_1._4.Views     5      9viewsmodels!clr-namespace:Cas_1._4.ViewModels     5      .��5      � 
����� �  � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��50 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      .+�5   
   �� $��0��6   $��Database Name (Keyspace):��6   �� 5   
   -   5         ��Name$ DatabaseTextBox��5      $��0��6+   $��1��68   $��5q�6H   �� 5    
   $��1��6   $��Node ID:��6   �� 5!   
   -   5        $ NodeIdTextBox��5!      $��1��6)   $��1��66   $��5q�6F   �� 5#   
   $��2��6   $��1��6"   .+�5$      �� 86Синхронное добавление данных66   �� 5%      <:Значения генерируются рандомно68   5&      �� 5)   
   -   5        $��3��5)      $��1��6   $��Insert Data��61   $��Center=�6`   $��5q�6}   �� 5+   
   -   5         ��Name$ ResultTextBox��5+      $��4��6+   $��1��68   $
��Wrap=�6H   5,      5-      