#pragma once //Блок глобальных объявлений
#include "Moves.h" // Класс со списком ходов 

//----- Перечисления ----------------------------------------------------------------
enum eColor {colWhite, colBlack}; //Цвета
enum eLevel {llow = 3, lmedium = 4, lhigh = 5}; //Уровни игры (с максимальной глубиной просчета в полуходах)
enum eResults {reOk, reBed, reKill, reGameOver, reDrawGame}; // reNot, reAlien, reBlock, Результаты обработки команд
//enum eCommands {cmExit,  cmRotate, cmComputer, cmNew, cmMove, cmLevel, cmError}; // Команды

//----- Глобальные функции / процедуры ----------------------------------------------
		
inline bool fFigure(const char F){return abs(F) > 1;}; //Возвращает наличие / отсутствие фигуры (true = в наличии) по значению поля
inline bool fFColor(const char F){return (F >> 8) * -1;}; //Возвращает цвет фируры (false = белый) по значению поля
inline void pSField(char (*DF)[8], const char (*SF)[8]) //Копирует значения поля игры из источника SF в приемник DF
	{for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) DF[i][j] = SF[i][j];}; // В циклах обхода поля - кппируем элементы.
inline int fRotating(const int i, const bool R){return i * !R + (7 - i) * R;}; //Возвращает индекс элемента поля i, развернутый наизнанку (при R == true) по принципу: 8 = 1, 7 = 2, и тп.

//----- Структура - Буфер обмена (временное хранилище данных)-------------------------
struct CBuffer
{
	Moves Mvs; //Список ходов
	char Field[8][8]; // Поле игры (8х8)
	bool Master; //Цвет игрока текущего хода
	void Add(Move& Mv){	Mvs.Add(Mv);}; //Добавить ход к списку
	void Set(char (*SF)[8]){pSField(Field, SF);}; //Установить значения поля игры Field из источника SF
	void Set(char (*SF)[8], bool Mst){Set(SF);  Master = Mst;};//Установить значения поля игры из SF и цвет игрока текущего хода
	void Addl(Moves& SMvs) //переписать (обновить) список ходов из источника SMvs
	{
		if(!SMvs.ToStart()) return; //Если SMvs пуст (нет первого элемента) - возврат
		Mvs.Clear(); Move Mv; //Очищаем список и создаем переменную типа Ход
		while(SMvs.Next(Mv)) Mvs.Add(Mv); //Пока есть следующий элемент SMvs, добавляем его в список
	}; // - Addl
}; // -CBuffer