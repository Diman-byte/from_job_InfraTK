   M S B A M L   `   `   `  ����B  >Cas_1.4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Cas_1._4Cas_1._4  5   	   /clr-namespace:Cas_1._4.ViewsCas_1._4.Views  5   	       Cas_1._4.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Cas_1._4     5   	   .viewsclr-namespace:Cas_1._4.Views     5   	     � Title$  Cassandra Data Management��5   	   $	��450��6,   $	��800��69   .��5      � 
������� � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      �� 5      �� 5      �� 5      .+�5"   
   o� $��0��6   $��5��6   .@�5#      m� $
��File��6   .@�5$      m� -   5        $��Connect��5$      6,   m� 5%      -   5        $��
Disconnect��5%      6/   �� 5&      m� 5'      -   5        $
��Exit��5'      6)   5(      m� 5)      $
��Data��6   .@�5*      m� -   5        $��Insert Random Data��5*      67   m� 5+      -   5        $��Insert Random Data Async��5+      6=   m� 5,      -   5        $@��:Посмотреть исторические данные��5,      6C   �� 5-      m� 5.      -   5        $��Create KeySpace��5.      64   m� 5/      -   5        $��Create Column Family��5/      69   �� 50      m� 51      -	   5        $*��$Очистка таблиц узла��51      68   m� 52      -
   5        $P��JУдаление таблиц в ключевом пространстве��52      6L   53      m� 54      $��Events��6   .@�55      m� -   5        $��Manage Events��55      62   56      57      �� 5:   
   -   5         ��Name$ 	IPTextBox��5:      $��1��6'   $��1��64   $��3��6D   $
��Wrap=�6X   $��15(�6l   $��
0, 2, 4, 2q�6z   ��5;      �� 5<       �� Color$ White��6"   5>      �� 5@   
   -   5         ��Name$ Placeholder��5@   [   $��2��6   $��0��6&   $��5��66   $��6��6J   6[   5C      5D      