import sqlite3

conn = sqlite3.connect('quiz.db')
cur = conn.cursor()

def init():
    cur.execute('''CREATE TABLE IF NOT EXISTS users(
        id INTEGER PRYMARY KEY AUTOINCREMENT,
        tg_id INTEGER,
        high_score INTEGER          
    )''')

    cur.execute('''CREATE TABLE IF NOT EXISTS questions(
        id INTEGER PRYMARY KEY AUTOINCREMENT,
        question TEXT,
        ans1 TEXT,
        ans2 TEXT,
        ans3 TEXT,
        ans4 TEXT,
        correct TEXT
    )''')

    conn.commit()

def save_user(user_id, score):
    cur.execute('INSERT INTO users(tg_id, high_score) VALUES(?, ?)', [user_id, score])
    conn.commit()

def get_all_questions():
    cur.execute('SELECT * FROM questions')
    return cur.fetchall()

def save_question(question, ans1, ans2, ans3, ans4, correct):
    cur.execute('INSERT INTO questions(question, ans1, ans2, ans3, ans4, correct VALUES (?,?,?,?,?,?))', [question, ans1, ans2, ans3, ans4, correct])
    conn.commit()
