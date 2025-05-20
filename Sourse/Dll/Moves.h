#pragma once // Класс со списком ходов
#include "Points.h" // Координаты

// --------- Структура - Ход (со списком координат) -----------------------
struct Move
{
	int Weight, OppCount; // Вес хода, количество возможных ходов противника после него
	bool Kill; Points Pts; // Флаг рубящего хода, Список перемещений
	Move(){}; // Конструктор по умолчанию
	Move(const Point& Pt) {Kill = OppCount = Weight = 0; Pts.Add(Pt);}; // Конструктор с начальной координатой фигуры
	Move(Move& M){Kill = M.Kill; OppCount = M.OppCount; Weight = M.Weight; Pts = M.Pts;}; // Конструктор копирования
}; // - Move

// ----------- Класс - Список ходов ---------------------------
class Moves
{
	struct MList // Элемент списка
	{
		Move Mv; MList* pPrev; MList* pNext; // Элемент - ход, адрес предыдущего, адрес следующего элементов
		MList(MList* pPrv, Move& M) : Mv(M){pNext = 0; pPrev = pPrv; if(pPrv) pPrv -> pNext = this;}; // Конструктор с установкой хода М
	};// - MList
				// -----------------------------------------
	int nCount; MList* pFirst; MList* pLast; MList* pCur; MList* pDel; // Количество элементов, первый, последний, текущий и удаляемый элементы списка
	void ReSet(Move& DM, Move& SM) // Копировать Ход из источника SM в приемник DM 
		{DM.OppCount = SM.OppCount; DM.Weight = SM.Weight; DM.Kill = SM.Kill; DM.Pts = SM.Pts;}; // копируем поля объекта
				// -----------------------------------------
public:
	bool Kills; // Есть ли в списке рубящие ходы (для сортировки)
	Moves(){Kills = nCount = 0; pFirst = pLast = pCur = 0;}; // Конструктор по умолчанию
				// -----------------------------------------
	void Clear() // Очитстить список
	{
		if(!nCount) return; MList* mL = pFirst; // Если список пуст - возврат... Копия текущего элемента
		for(int i = 0; i < nCount; i++){MList* pDlt = mL; mL = mL -> pNext; delete pDlt;} // Удаляем поочередно элементы
		Kills = nCount = 0; pDel = pCur = pFirst = pLast = 0; // Обнуляем указатели
	}; // - Clear
				// -----------------------------------------
	void Add(Move& Mv) // Добавить в конец списока ход Mv
	{
		MList* mL = new MList(pLast, Mv); pCur = pLast = mL;  // Создаем элемент из указанного значения и устанавливаем в хвост. Устанавливаем указатели.
		if(!nCount) pFirst = mL; pDel = 0; nCount++; // если элемент первый, устанавливаем указатель. Обнуляем указатель на удалаемый элемент. Увеличиваем счетчик.
	}; // - Add
				// -----------------------------------------
	bool Delete() // Удалить элемент списка (pDel)
	{
		if(!pDel) return false; // Если не установлен указатель - возврат ложь.
		if(pDel == pFirst) pFirst = pDel -> pNext; // если удаляемый элемент первый - меняем указатель
		if(pDel -> pNext) pDel -> pNext -> pPrev = pDel -> pPrev; // если нужно, меняем указатели
		if(pDel -> pPrev) pDel -> pPrev -> pNext = pDel -> pNext; // соседних элементов
		delete pDel; nCount--; return true; // удаляем элемент, уменьшаем счетчик, возврат - истина
	}; // - Delete
				// -----------------------------------------
	void Sorted(int n) // Сортировать список (по убыванию веса хода) начиная с элемента n
	{
		MList* mL = pFirst; MList* cmL;  MList* tmL; // Текущий элемент, наименьший (первый) и первый элементы
		for(int m = 1; mL; m++) // Цикл для m, пока mL в пределах списка
		{
			if(m == n) cmL = tmL = mL; // Если дошли до элемента n, устанавливаем наименьший и первый элементы
			if(m > n) // Если номер элемента больше n
				if((cmL -> Mv.Weight < mL -> Mv.Weight) || //Если вес наименьший хода меньше веса текущего
				((cmL -> Mv.Weight == mL -> Mv.Weight) && (cmL -> Mv.OppCount > mL -> Mv.OppCount))) // или весы равны, но количество ходов противника больше
					cmL = mL; // Устанавливаем наименьший злемент на текущий
			mL = mL -> pNext; // Следующий элемент
		} // - for
		if(tmL == cmL) return; // Если наименьший элемент равен первому - возврат
		Move tMv(tmL -> Mv); ReSet(tmL -> Mv, cmL -> Mv); ReSet(cmL -> Mv, tMv); // Создаем копию хода и ч\з нее меняем первый и наименьший элементы местами
	}; // - Sorted
				// -----------------------------------------
	 int Count(){pDel = 0; return nCount;}; // Вернуть количество элементов списка 
	bool ToStart(){pDel = 0; if(!pFirst) return false; pCur = pFirst; return true;}; // Перейти к первому элементу списка
	bool First(Move& M){pDel = 0; if(!pFirst) return false; M = pFirst -> Mv; return true;}; // Получить первый элемент списка
	void Set(Move& M){if(!pDel) return; pDel -> Mv.OppCount = M.OppCount; pDel -> Mv.Weight = M.Weight;}; // Установить в текущий элемент списка значения из хода Mv
	bool Next(Move& M){pDel = pCur; if(!pCur) return false; M = pCur -> Mv; pCur = pCur -> pNext; return true;}; // Получить следующий элемент списка
	~Moves(){Clear();}; //Деструктор (с очисткой списка)
}; // - Moves
// -----------------------------------------