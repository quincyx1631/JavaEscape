
try:
    age = int(input("Input your Age: "))
except ValueError as ex:
    print("You didn't input a VALID Age")
    print(ex)
    print(type(ex))
else:
    print("No Exception")
    birth_year = 2024 - age
    print("Your birthyear  is " + "\033[33m" + (str(birth_year) + "\033[0m"))  
   
