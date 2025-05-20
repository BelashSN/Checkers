#pragma once // Класс со списком координат
#include <cstdlib> // Для использования некоторых мат. функций

// ----------- Структура - Координата  ---------------------------
struct Point
{
	int X, Y; // Координаты адреса 
	Point(){X = -1; Y = -1;}; // Конструктор по умолчанию
	Point(int newX, int newY){X = newX; Y = newY;}; // Конструктор с координатами
	Point& Set(int newX, int newY){X = newX; Y = newY; return *this;}; // Установить координаты адреса
	bool operator==(const Point& Pt){return ((X == Pt.X) && (Y == Pt.Y));}; // Оператор "Равно"
	bool operator!=(const Point& Pt){return ((X != Pt.X) || (Y != Pt.Y));}; // Оператор "Не Равно"
	Point& operator>>(int n){X += abs(n); Y += n; return *this;}; //Оператор "Сместить координату вправо"
	Point& operator<<(int n){X -= abs(n); Y += n; return *this;}; //Оператор "Сместить координату влево" 
}; // - Point

// -----------  Класс - Список координат  ------------------
class Points
{	
	struct PList //Элемент списка
	{
		Point Pt; PList* pNext; // Элемент - координата, адрес следующего элемента
		PList(PList* pPrv, const Point& P) : Pt(P){pNext = 0; if(pPrv) pPrv -> pNext = this;};  // Конструктор с установкой координаты Р
	}; // - PList
				// -----------------------------------------
	int nCount; PList* pFirst; PList* pLast; PList* pCur; // Количество элементов, первый, последний и текущий элементы списка 
				// -----------------------------------------
public:
	Points(){nCount = 0; pFirst = pLast = pCur = 0;}; // Конструктор по умолчанию
				// -----------------------------------------
	void Clear() // Очитстить список
	{
		if(!nCount) return; PList* pL = pFirst; // Если список пуст - возврат... Копия текущего элемента
		for(int i = 0; i < nCount; i++){PList* pDlt = pL; pL = pL -> pNext; delete pDlt;} // Удаляем поочередно элементы
		nCount = 0; pCur = pFirst = pLast = 0; // Обнуляем указатели
	}; // - Clear
				// -----------------------------------------
	void Add(const Point& Pt) // Добавить в хвост списока координату Pt
	{
		PList* pL = new PList(pLast, Pt); pCur = pLast = pL; // Создаем элемент из указанного значения и устанавливаем в хвост. Устанавливаем указатели.
		if(!nCount) pFirst = pL; nCount++; // если элемент первый, устанавливаем указатель. Увеличиваем счетчик.
	}; // - Add
				// -----------------------------------------
	void Addl(Points& Pts) // Добавить в конец списока координаты из списка Pts
	{
		if(!Pts.ToStart()) return; // Если новый список пуст - возврат.
		Point Ptt; while(Pts.Next(Ptt)) Add(Ptt); // добавляем поочередно элементы нового списка в хвост старого.
	}; // - Addl
				// -----------------------------------------
	Points& operator=(Points& Pts) //Оператор Присваивания
	{
		if(this == &Pts) return *this; // Если ссылка на собсвтенный объект - возврат
		Clear(); Addl(Pts);  return *this; // Очищаем список, добавляем в хвост новый список, возвращаем текущий объект.
	}; // - operator=
				// -----------------------------------------
	int Count(){return nCount;}; // Вернуть количество элементов списка 
	bool ToStart(){if(!pFirst) return false; pCur = pFirst; return true;}; // Перейти к первому элементу списка
	bool Last(Point& P){if(!pLast) return false; P = pLast -> Pt; return true;}; // Получить последний элемент списка
	bool First(Point& P){if(!pFirst) return false; P = pFirst -> Pt; return true;}; // Получить первый элемент списка
	bool Next(Point& P){if(!pCur) return false; P = pCur -> Pt; pCur = pCur -> pNext; return true;};// Получить следующий элемент списка
	~Points(){Clear();}; // Деструктор (с очисткой списка)
}; // - Points
// -----------------------------------------