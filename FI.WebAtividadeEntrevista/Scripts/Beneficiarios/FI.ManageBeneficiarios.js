let beneficiarios = [];
let beneficiariosExcluidos = [];

$(document).ready(function () {
    $('#cpfBeneficiario').each(function () {
        $(this).mask('000.000.000-00')
    });

    $('#incluirBeneficiario').click(incluirBeneficiario);

    renderBeneficiarios();
})

var isEdit = $('#btnModalBeneficiario').data('is-edit');

if (isEdit === 'True') {
    if (beneficiariosObj) {
        try {
            beneficiarios = beneficiariosObj;
        } catch (e) {
            console.error("Erro ao parsear BeneficiariosJson:", e);
            beneficiarios = [];
        }
    }
}

function incluirBeneficiario() {
    const cpf = $('#cpfBeneficiario').val().replace(/[.\-]/g, '');
    const nome = $('#nomeBeneficiario').val();

    if (!cpf || !nome) {
        alert("Preencha CPF e Nome");
        return;
    }

    if (beneficiarios.some(b => b.CPF === cpf)) {
        alert("CPF já incluído.");
        return;
    }

    beneficiarios.push({ CPF: cpf, Nome: nome });
    renderBeneficiarios();
    $('#cpfBeneficiario').val('');
    $('#nomeBeneficiario').val('');
}

function alterarBeneficiario(cpf) {
    const beneficiario = beneficiarios.find(x => x.CPF === cpf);
    if (!beneficiario) return;

    $('.btn-alterar').prop('disabled', true);
    $('.btn-excluir').prop('disabled', true);
    $('#cpfBeneficiario').val(beneficiario.CPF);
    $('#nomeBeneficiario').val(beneficiario.Nome);

    $('#incluirBeneficiario').text('Atualizar').off('click').click(function () {
        beneficiario.Nome = $('#nomeBeneficiario').val();
        beneficiario.CPF = $('#cpfBeneficiario').val().replace(/[.\-]/g, '');

        if (!beneficiario.CPF || !beneficiario.Nome) {
            alert("Preencha CPF e Nome");
            return;
        }

        if (beneficiario.Id !== undefined)
            beneficiario.IsEdit = true;
        
        renderBeneficiarios();
        $('#cpfBeneficiario').val('');
        $('#nomeBeneficiario').val('');
        $(this).text('Incluir').off('click').click(incluirBeneficiario);
    });
}

function excluirBeneficiario(cpf) {
    const beneficiario = beneficiarios.find(x => x.CPF === cpf);
    if (!beneficiario) return;

    if (beneficiario.Id !== undefined) {
        beneficiario.IsDelete = true;
        beneficiariosExcluidos.push(beneficiario);
    }

    beneficiarios = beneficiarios.filter(b => b.CPF !== cpf);
    renderBeneficiarios();
}

function renderBeneficiarios() {
    let html = '';
    for (let beneficiario of beneficiarios) {
        html += `<tr>
                        <td>${beneficiario.CPF}</td>
                        <td>${beneficiario.Nome}</td>
                        <td style='width: auto;'>
                            <button class='btn btn-sm btn-primary btn-alterar' style='margin-right: 5px;' onclick="alterarBeneficiario('${beneficiario.CPF}')">Alterar</button>
                            <button class='btn btn-sm btn-danger btn-excluir' onclick="excluirBeneficiario('${beneficiario.CPF}')">Excluir</button>
                        </td>
                     </tr>
                 `;
    }
    $('#listaBeneficiarios').html(html);

    const todosBeneficiarios = [...beneficiarios, ...beneficiariosExcluidos];
    $('#BeneficiariosJson').val(JSON.stringify(todosBeneficiarios)); // sincroniza com input hidden
}