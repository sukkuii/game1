number = int(input("Введите число: "))

if 10 <= number <= 99:
    tens = number // 10
    units = number % 10

    sum_of_digits = tens + units

    print(f"{number} = {tens} + {units} = {sum_of_digits}")фвпрфф
else:
    print("Ошибка: введите двузначное число.")