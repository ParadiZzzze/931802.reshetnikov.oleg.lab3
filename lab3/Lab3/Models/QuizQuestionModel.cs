using System;
using Microsoft.AspNetCore.Hosting;

namespace Lab3.Models {
    public class QuizQuestionModel {
        public int X { get; set; }
        public int Y { get; set; }
        public string Operation { get; set; }

        public int CorrectAnswer {
            get {
                return Operation switch {
                    "+" => X + Y,
                    "-" => X - Y,
                    "*" => X * Y,
                    "/" when Y != 0 => X / Y,
                    "/" when Y == 0 => throw new Exception("Dividing by zero"),
                    _ => throw new Exception("Invalid operation")
                };
            }
        }

        public int? UserAnswer { get; set; }

        public bool AnswerIsCorrect {
            get { return UserAnswer is { } a && a == CorrectAnswer; }
        }

        public override string ToString() {
            return Operation switch {
                "+" => $"{X} + {Y} = ",
                "-" => $"{X} - {Y} = ",
                "*" => $"{X} * {Y} = ",
                "/" when Y != 0 => $"{X} / {Y} = ",
                "/" when Y == 0 => throw new Exception("Dividing by zero"),
                _ => throw new Exception("Invalid operation")
            };
        }

        public static QuizQuestionModel RandomQuizQuestion {
            get {
                var random = new Random();
                return new QuizQuestionModel{
                    X = random.Next(0, 10),
                    Y = random.Next(1, 10),
                    Operation = random.Next(4) switch {
                        0 => "+",
                        1 => "-",
                        2 => "*",
                        3 => "/",
                        _ => throw new Exception("Random number is too big")
                    }
                };
            }
        }
    }
}
