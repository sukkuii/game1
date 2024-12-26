from aiogram import types, Dispatcher, Bot
from aiogram.utils import executor
from aiogram.dispatcher.filters.state import StatesGroup, State
from aiogram.contrib.fsm_storage.memory import MemoryStorage
import db

bot = Bot(token='', parse_mode="HTML")
dp = Dispatcher(bot, storage=MemoryStorage())
db.init()
class UserQuiz(StatesGroup):
    user_a1 = State()
    user_a2 = State()
    user_a3 = State()
    user_a4 = State()
    user_a5 = State()

async def start(message: types.Message):
    await message.answer('Добро пожаловать в AE! Сыграйте в мини-игру, чтобы получить промокод. В чат придёт уведомление, как только раунд начнётся!')
    db.save_user(message.chat.id, 0)

async def game(message: types.Message, state):
    question = db.get_all_questions()[0]
    keyboard = types.ReplyKeyboardMarkup()
    keyboard.add(question[2], question[3])
    keyboard.add(question[4], question[5])
    await message.answer(f'Вопрос №1. {question[1]}', reply_markup=keyboard)
    await UserQuiz.user_a1.set()
    await state.update_data(correct = question[6])
    await state.update_data(points = 0)

async def on_user_a1(message: types.Message, state):
    data = await state.get_data()
    if data.get('correct') == message.text:
        points = data.get('points') + 1
        await state.update_data(points = points)
    question = db.get_all_questions()[1]
    keyboard = types.ReplyKeyboardMarkup()
    keyboard.add(question[2], question[3])
    keyboard.add(question[4], question[5])
    await message.answer(f'Вопрос №2. {question[1]}', reply_markup=keyboard)
    await UserQuiz.user_a1.set()
    await state.update_data(correct = question[6])

async def on_user_a2(message: types.Message, state):
    data = await state.get_data()
    if data.get('correct') == message.text:
        points = data.get('points') + 1
        await state.update_data(points = points)
    question = db.get_all_questions()[2]
    keyboard = types.ReplyKeyboardMarkup()
    keyboard.add(question[2], question[3])
    keyboard.add(question[4], question[5])
    await message.answer(f'Вопрос №3. {question[1]}', reply_markup=keyboard)
    await UserQuiz.user_a2.set()
    await state.update_data(correct = question[6])

async def on_user_a3(message: types.Message, state):
    data = await state.get_data()
    if data.get('correct') == message.text:
        points = data.get('points') + 1
        await state.update_data(points = points)
    question = db.get_all_questions()[3]
    keyboard = types.ReplyKeyboardMarkup()
    keyboard.add(question[2], question[3])
    keyboard.add(question[4], question[5])
    await message.answer(f'Вопрос №4. {question[1]}', reply_markup=keyboard)
    await UserQuiz.user_a3.set()
    await state.update_data(correct = question[6])

async def on_user_a4(message: types.Message, state):
    data = await state.get_data()
    if data.get('correct') == message.text:
        points = data.get('points') + 1
        await state.update_data(points = points)
    question = db.get_all_questions()[4]
    keyboard = types.ReplyKeyboardMarkup()
    keyboard.add(question[2], question[3])
    keyboard.add(question[4], question[5])
    await message.answer(f'Вопрос №5. {question[1]}', reply_markup=keyboard)
    await UserQuiz.user_a4.set()
    await state.update_data(correct = question[6])

async def on_user_a5(message: types.Message, state):
    data = await state.get_data()
    points = data.get('points')
    if data.get('correct') == message.text:
        points +=  1
        await state.update_data(points = points)
    
    await message.answer(f'Вы ответили правильно на {points} вопросов из пяти')
    await state.finish()


def register_handlers(dp: Dispatcher):
    dp.register_message_handler(start, commands="start", state="*")
    dp.register_message_handler(game, commands="startgame", state="*")
    dp.register_message_handler(on_user_a1, state=UserQuiz.user_a1)
    dp.register_message_handler(on_user_a2, state=UserQuiz.user_a2)
    dp.register_message_handler(on_user_a3, state=UserQuiz.user_a3)
    dp.register_message_handler(on_user_a4, state=UserQuiz.user_a4)
    dp.register_message_handler(on_user_a5, state=UserQuiz.user_a5)

register_handlers(dp)
executor.start_polling(dp, skip_updates=True)
