   M S B A M L   `   `   `  ����  Cas_1.42    ,Cas_1._4.Views.InitializeHistDBColumnsWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Initialize HistDB Columns��5   	   $	��300��6+   $	��450��68   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��5q�6;   .+�5      �� $-��'Название базы данных:��6   �� 5      -   5         ��Name$ DatabaseNameTextBox��5      $��5q�63   $	��167��6>   $��17��6J   5      �� 5   
   $��1��6   $��5q�6"   .+�5      �� $e��_Список идентификаторов узлов (разделенных запятой):��6   �� 5      -   5        $ NodeIdsTextBox��5      $��5q�6.   $	��100��69    �� TextWrapping$
 Wrap=�6F   5      �� 5   
   -   5        $5��/Инициализировать столбцы��5      $��2��64   $��5q�6g   �� 5   
   -   5         ��Name$ ResultTextBox��5      $��3��6+   $��5q�68   $
��Wrap=�6C   5      5      