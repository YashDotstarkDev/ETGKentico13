using FluentValidation;

namespace ETG.Data.Forms
{
    public interface IBizformEntry<T>
    {
        void Initialize(AbstractValidator<T> validator, T bizformItem);
        bool Validate();

        T Save();


        bool Submit();
    }
}